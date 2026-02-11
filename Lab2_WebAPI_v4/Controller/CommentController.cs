using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lab2_WebAPI_v4.Controller
{
    /// <summary>
    /// Handles operations related to comments on blog posts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        /// <summary>
        /// Constructor – injects CommentRepo via dependency injection.
        /// </summary>
        public CommentController(ICommentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Extracts the logged-in user's ID from JWT token.
        /// </summary>
        private int GetUserIdFromToken()
        {
            return int.Parse(User.FindFirst("UserID")!.Value);
        }

        // -------------------- GET COMMENTS BY POST --------------------

        /// <summary>
        /// Returns all comments for a specific post.
        /// </summary>
        /// <param name="postId">ID of the post</param>
        [HttpGet("{postId}")]
        [SwaggerOperation(
            Summary = "Get comments for a post",
            Description = "Returns all comments belonging to a specific blog post."
        )]
        [ProducesResponseType(typeof(List<Comment>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments(int postId)
        {
            var comments = await _service.GetByPostAsync(postId);
            return Ok(comments);
        }

        // -------------------- ADD COMMENT --------------------

        /// <summary>
        /// Adds a new comment to a post.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Add comment",
            Description = "Creates a new comment for a blog post. Users cannot comment on their own posts."
        )]
        [ProducesResponseType(typeof(Comment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.AddAsync(comment, GetUserIdFromToken());

                return Created("", comment);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("own post"))
                    return Forbid();

                if (ex.Message.Contains("not found"))
                    return NotFound();

                return BadRequest(ex.Message);
            }
        }

        // -------------------- DELETE COMMENT --------------------

        /// <summary>
        /// Deletes a comment. Only the comment owner can delete it.
        /// </summary>
        /// <param name="commentId">ID of the comment</param>
        [HttpDelete("{commentId}")]
        [SwaggerOperation(
            Summary = "Delete comment",
            Description = "Deletes a comment. Only the user who created the comment can delete it."
        )]
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
}


