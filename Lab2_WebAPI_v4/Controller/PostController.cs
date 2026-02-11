using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.DTOs;
using Lab2_WebAPI_v4.DTOs.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lab2_WebAPI_v4.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _service;

        public PostController(IPostService service)
        {
            _service = service;
        }

        private int GetUserIdFromToken()
        {
            return int.Parse(User.FindFirst("UserID")!.Value);
        }

        // -------------------- GET ALL POSTS --------------------

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all posts",
            Description = "Returns a list of all blog posts."
        )]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _service.GetAllAsync();
            return Ok(posts);
        }

        // -------------------- ADD POST --------------------

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create post",
            Description = "Creates a new blog post for the logged-in user."
        )]
        public async Task<IActionResult> AddPost([FromBody] CreatePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPost = await _service.AddAsync(dto, GetUserIdFromToken());

            return Created("", createdPost);
        }

        // -------------------- UPDATE POST --------------------

        [HttpPut]
        [SwaggerOperation(
            Summary = "Update post",
            Description = "Updates a blog post. Only the owner can update it."
        )]
        public async Task<IActionResult> UpdatePost([FromBody] PostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = await _service.UpdateAsync(dto, GetUserIdFromToken());

            if (!ok)
                return Forbid();

            return NoContent();
        }

        // -------------------- DELETE POST --------------------

        [HttpDelete("{postId}")]
        [SwaggerOperation(
            Summary = "Delete post",
            Description = "Deletes a blog post. Only the owner can delete it."
        )]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var ok = await _service.DeleteAsync(postId, GetUserIdFromToken());

            if (!ok)
                return Forbid();

            return NoContent();
        }

        // -------------------- SEARCH BY TITLE --------------------

        [HttpGet("search/title")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("Title search term is required.");

            var posts = await _service.SearchByTitleAsync(title);

            return Ok(posts);
        }

        // -------------------- SEARCH BY CATEGORY --------------------

        [HttpGet("search/category")]
        public async Task<IActionResult> SearchByCategory([FromQuery] int categoryId)
        {
            if (categoryId <= 0)
                return BadRequest("Valid categoryId is required.");

            var posts = await _service.SearchByCategoryAsync(categoryId);

            return Ok(posts);
        }
    }
}


