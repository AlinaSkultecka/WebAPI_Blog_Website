using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.Comment;
using Lab2_WebAPI_v4.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// API controller responsible for managing blog post comments.
/// Requires authentication for all endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentController"/> class.
    /// </summary>
    public CommentController(ICommentService service)
    {
        _service = service;
    }

    /// <summary>
    /// Extracts the authenticated user's ID from the JWT token.
    /// </summary>
    private int GetUserIdFromToken()
    {
        return int.Parse(User.FindFirst("UserID")!.Value);
    }

    // -------------------- GET COMMENTS BY POST --------------------

    /// <summary>
    /// Retrieves all comments associated with a specific post.
    /// </summary>
    [HttpGet("{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComments(int postId)
    {
        var comments = await _service.GetByPostAsync(postId);
        return Ok(comments);
    }

    // -------------------- ADD COMMENT --------------------

    /// <summary>
    /// Creates a new comment for a post.
    /// Users are not allowed to comment on their own posts.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddComment([FromBody] CreateCommentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _service.AddAsync(dto, GetUserIdFromToken());
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (InvalidOperationException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // -------------------- DELETE COMMENT --------------------

    /// <summary>
    /// Deletes a comment.
    /// Only the user who created the comment can delete it.
    /// </summary>
    [HttpDelete("{commentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var ok = await _service.DeleteAsync(commentId, GetUserIdFromToken());

        if (!ok)
            return Forbid();

        return NoContent();
    }
}