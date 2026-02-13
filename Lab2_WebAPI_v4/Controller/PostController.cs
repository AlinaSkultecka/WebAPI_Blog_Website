using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.Post;
using Lab2_WebAPI_v4.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// API controller responsible for managing blog posts.
/// All endpoints require authentication.
/// </summary>
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

    /// <summary>
    /// Extracts authenticated user's ID from JWT token.
    /// </summary>
    private int GetUserIdFromToken()
    {
        return int.Parse(User.FindFirst("UserID")!.Value);
    }

    // -------------------- GET ALL POSTS --------------------

    /// <summary>
    /// Retrieves all blog posts.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PostDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _service.GetAllAsync();
        return Ok(posts);
    }

    // -------------------- ADD POST --------------------

    /// <summary>
    /// Creates a new blog post for the authenticated user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPost([FromBody] CreatePostDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdPost = await _service.AddAsync(dto, GetUserIdFromToken());

            return Created("", createdPost);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // -------------------- UPDATE POST --------------------

    /// <summary>
    /// Updates a blog post. Only the owner can update it.
    /// </summary>
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
            var ok = await _service.UpdateAsync(dto, GetUserIdFromToken());

            if (!ok)
                return Forbid();

            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // -------------------- DELETE POST --------------------

    /// <summary>
    /// Deletes a blog post. Only the owner can delete it.
    /// </summary>
    [HttpDelete("{postId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeletePost(int postId)
    {
        var ok = await _service.DeleteAsync(postId, GetUserIdFromToken());

        if (!ok)
            return Forbid();

        return NoContent();
    }

    // -------------------- SEARCH BY TITLE --------------------

    /// <summary>
    /// Searches posts by title (partial match).
    /// </summary>
    [HttpGet("search/title")]
    public async Task<IActionResult> SearchByTitle([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Title search term is required.");

        var posts = await _service.SearchByTitleAsync(title);

        return Ok(posts);
    }

    // -------------------- SEARCH BY CATEGORY --------------------

    /// <summary>
    /// Retrieves posts belonging to a specific category.
    /// </summary>
    [HttpGet("search/category")]
    public async Task<IActionResult> SearchByCategory([FromQuery] int categoryId)
    {
        if (categoryId <= 0)
            return BadRequest("Valid categoryId is required.");

        var posts = await _service.SearchByCategoryAsync(categoryId);

        return Ok(posts);
    }
}