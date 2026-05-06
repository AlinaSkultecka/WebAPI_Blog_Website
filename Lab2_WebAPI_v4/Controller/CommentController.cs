using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_WebAPI_v4.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
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

        [HttpGet("{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments(int postId)
        {
            var comments = await _service.GetByPostAsync(postId);
            return Ok(comments);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromToken();

            if (userId == null)
                return Unauthorized("UserID claim is missing from token.");

            try
            {
                await _service.AddAsync(dto, userId.Value);
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

        [HttpDelete("{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = GetUserIdFromToken();

            if (userId == null)
                return Unauthorized("UserID claim is missing from token.");

            var ok = await _service.DeleteAsync(commentId, userId.Value);

            if (!ok)
                return Forbid();

            return NoContent();
        }
    }
}