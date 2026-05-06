using Lab2_WebAPI_v4;
using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_WebAPI_v4.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _service;
        private readonly BlobLoggingService _logger;
        private readonly ILogger<PostController> _appLogger;

        public PostController(
            IPostService service,
            BlobLoggingService logger,
            ILogger<PostController> appLogger)
        {
            _service = service;
            _logger = logger;
            _appLogger = appLogger;
        }

        private int? GetUserIdFromToken()
        {
            var claim = User.FindFirst("UserID");

            if (claim == null)
                return null;

            if (!int.TryParse(claim.Value, out var userId))
                return null;

            return userId;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PostDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _service.GetAllAsync();

            _appLogger.LogInformation("All posts retrieved.");

            return Ok(posts);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddPost([FromBody] CreatePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromToken();

            if (userId == null)
                return Unauthorized("UserID claim is missing from token.");

            try
            {
                var createdPost = await _service.AddAsync(dto, userId.Value);

                await _logger.LogAsync($"User {userId.Value} created a post: {dto.Title}");
                _appLogger.LogInformation("User {UserId} created a post: {Title}", userId.Value, dto.Title);

                return Created("", createdPost);
            }
            catch (ArgumentException ex)
            {
                _appLogger.LogWarning(ex, "Invalid post creation request.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromToken();

            if (userId == null)
                return Unauthorized("UserID claim is missing from token.");

            try
            {
                var ok = await _service.UpdateAsync(dto, userId.Value);

                if (!ok)
                {
                    _appLogger.LogWarning(
                        "User {UserId} tried to update post {PostId} without permission.",
                        userId.Value,
                        dto.PostID);

                    return Forbid();
                }

                await _logger.LogAsync($"User {userId.Value} updated post with id: {dto.PostID}");
                _appLogger.LogInformation("User {UserId} updated post {PostId}", userId.Value, dto.PostID);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                _appLogger.LogWarning(ex, "Invalid post update request.");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{postId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = GetUserIdFromToken();

            if (userId == null)
                return Unauthorized("UserID claim is missing from token.");

            var ok = await _service.DeleteAsync(postId, userId.Value);

            if (!ok)
            {
                _appLogger.LogWarning(
                    "User {UserId} tried to delete post {PostId} without permission.",
                    userId.Value,
                    postId);

                return Forbid();
            }

            await _logger.LogAsync($"User {userId.Value} deleted post with id: {postId}");
            _appLogger.LogInformation("User {UserId} deleted post {PostId}", userId.Value, postId);

            return NoContent();
        }

        [HttpGet("search/title")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("Title search term is required.");

            var posts = await _service.SearchByTitleAsync(title);

            _appLogger.LogInformation("Posts searched by title: {Title}", title);

            return Ok(posts);
        }

        [HttpGet("search/category")]
        public async Task<IActionResult> SearchByCategory([FromQuery] int categoryId)
        {
            if (categoryId <= 0)
                return BadRequest("Valid categoryId is required.");

            var posts = await _service.SearchByCategoryAsync(categoryId);

            _appLogger.LogInformation("Posts searched by category: {CategoryId}", categoryId);

            return Ok(posts);
        }
    }
}