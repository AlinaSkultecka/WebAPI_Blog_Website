using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lab2_WebAPI_v4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostRepo _repo;

        public PostController(IPostRepo repo)
        {
            _repo = repo;
        }

        private int GetUserIdFromToken()
        {
            return int.Parse(User.FindFirst("UserID")!.Value);
        }

        [HttpGet]
        public IActionResult GetAllPosts()
        {
            return Ok(_repo.GetAllPosts());
        }

        [HttpPost]
        public IActionResult AddPost(Post post)
        {
            // Creating a post: client must NOT send PostID (identity column)
            if (post.PostID != 0)
                return BadRequest("Do not send PostID when creating a post.");

            // model validation (required fields etc.)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // extra check because missing int -> 0
            if (post.CategoryID <= 0)
                return BadRequest("CategoryID is required and must be > 0.");

            post.UserID = GetUserIdFromToken();
            _repo.AddPost(post);
            return Created();
        }


        [HttpPut]
        public IActionResult UpdatePost(Post post)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (post.CategoryID <= 0)
                return BadRequest("CategoryID is required and must be > 0.");

            post.UserID = GetUserIdFromToken();

            var ok = _repo.UpdatePost(post);

            if (!ok)
                return Forbid();

            return Ok();
        }


        [HttpDelete("{postId}")]
        public IActionResult DeletePost(int postId)
        {
            var userId = GetUserIdFromToken();

            var ok = _repo.DeletePost(postId, userId);

            if (!ok)
                return Forbid(); // or NotFound()

            return Ok();
        }

    }
}
