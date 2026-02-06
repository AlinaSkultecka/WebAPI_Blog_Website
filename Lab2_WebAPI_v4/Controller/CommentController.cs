using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_WebAPI_v4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepo _repo;

        public CommentController(ICommentRepo repo)
        {
            _repo = repo;
        }

        private int GetUserIdFromToken()
        {
            return int.Parse(User.FindFirst("UserID")!.Value);
        }

        [HttpGet("{postId}")]
        public IActionResult GetComments(int postId)
        {
            return Ok(_repo.GetCommentsByPost(postId));
        }

        [HttpPost]
        public IActionResult AddComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _repo.AddComment(comment, GetUserIdFromToken());
                return Created();
            }
            catch (Exception ex)
            {
                // simple mapping
                if (ex.Message.Contains("own post"))
                    return Forbid();

                if (ex.Message.Contains("not found"))
                    return NotFound();

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            var ok = _repo.DeleteComment(commentId, GetUserIdFromToken());

            if (!ok)
                return Forbid(); // or NotFound() depending on your lab rules

            return Ok();
        }
    }
}
