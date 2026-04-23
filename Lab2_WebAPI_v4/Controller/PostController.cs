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
        // This controller manages blog posts, allowing authenticated users to create, update, delete, and search posts.
        private readonly IPostService _service;
        private readonly BlobLoggingService _logger;
        private readonly ILogger<PostController> _appLogger;

        // Constructor with dependency injection for the post service, blob logging, and application logging.
        public PostController(
            IPostService service,
            BlobLoggingService logger,
            ILogger<PostController> appLogger)
        {
            _service = service;
            _logger = logger;          // Blob logging
            _appLogger = appLogger;    // Application Insights logging
        }

        // Helper method to extract the authenticated user's ID from the JWT token.
        private int GetUserIdFromToken()
        {
            return int.Parse(User.FindFirst("UserID")!.Value);
        }

        // -------------------- GET ALL POSTS --------------------
        [HttpGet]
        [ProducesResponseType(typeof(List<PostDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _service.GetAllAsync();
            _appLogger.LogInformation("All posts retrieved.");
            return Ok(posts);
        }

        // -------------------- GET POST BY ID --------------------
        [HttpPost]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPost([FromBody] CreatePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = GetUserIdFromToken();
                var createdPost = await _service.AddAsync(dto, userId);

                await _logger.LogAsync($"User {userId} created a post: {dto.Title}");
                _appLogger.LogInformation("User {UserId} created a post: {Title}", userId, dto.Title);

                return Created("", createdPost);
            }
            catch (ArgumentException ex)
            {
                _appLogger.LogWarning(ex, "Invalid post creation request.");
                return BadRequest(ex.Message);
            }
        }

        // -------------------- UPDATE POST --------------------
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = GetUserIdFromToken();
                var ok = await _service.UpdateAsync(dto, userId);

                if (!ok)
                {
                    _appLogger.LogWarning("User {UserId} tried to update post {PostId} without permission.", userId, dto.PostID);
                    return Forbid();
                }

                await _logger.LogAsync($"User {userId} updated post with id: {dto.PostID}");
                _appLogger.LogInformation("User {UserId} updated post {PostId}", userId, dto.PostID);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                _appLogger.LogWarning(ex, "Invalid post update request.");
                return BadRequest(ex.Message);
            }
        }

        // -------------------- DELETE POST --------------------
        [HttpDelete("{postId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = GetUserIdFromToken();
            var ok = await _service.DeleteAsync(postId, userId);

            if (!ok)
            {
                _appLogger.LogWarning("User {UserId} tried to delete post {PostId} without permission.", userId, postId);
                return Forbid();
            }

            await _logger.LogAsync($"User {userId} deleted post with id: {postId}");
            _appLogger.LogInformation("User {UserId} deleted post {PostId}", userId, postId);

            return NoContent();
        }

        // -------------------- SEARCH POSTS --------------------
        [HttpGet("search/title")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("Title search term is required.");

            var posts = await _service.SearchByTitleAsync(title);
            _appLogger.LogInformation("Posts searched by title: {Title}", title);

            return Ok(posts);
        }

        // -------------------- SEARCH POSTS BY CATEGORY --------------------
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