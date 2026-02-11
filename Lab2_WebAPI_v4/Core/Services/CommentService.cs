using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;

namespace Lab2_WebAPI_v4.Core.Services
{
    /// <summary>
    /// Handles business logic related to comments.
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly ICommentRepo _repo;
        private readonly IPostRepo _postRepo;

        public CommentService(ICommentRepo repo, IPostRepo postRepo)
        {
            _repo = repo;
            _postRepo = postRepo;
        }

        public async Task<List<Comment>> GetByPostAsync(int postId)
        {
            return await _repo.GetCommentsByPostAsync(postId);
        }

        public async Task AddAsync(Comment comment, int userId)
        {
            // Check if post exists
            var post = await _postRepo
                .GetAllPostsAsync(); // temporary simple way

            var targetPost = post.FirstOrDefault(p => p.PostID == comment.PostID);

            if (targetPost == null)
                throw new Exception("Post not found");

            // Business rule: cannot comment your own post
            if (targetPost.UserID == userId)
                throw new Exception("Cannot comment your own post");

            comment.UserID = userId;

            await _repo.AddCommentAsync(comment);
        }

        public async Task<bool> DeleteAsync(int commentId, int userId)
        {
            return await _repo.DeleteCommentAsync(commentId, userId);
        }
    }
}
