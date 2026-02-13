using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.Comment;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;

namespace Lab2_WebAPI_v4.Core.Services
{
    /// <summary>
    /// Handles business logic related to blog post comments.
    /// Responsible for enforcing comment rules before interacting with the repository layer.
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly ICommentRepo _repo;
        private readonly IPostRepo _postRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentService"/> class.
        /// </summary>

        public CommentService(ICommentRepo repo, IPostRepo postRepo)
        {
            _repo = repo;
            _postRepo = postRepo;
        }

        // -------------------- GET COMMENTS BY POST --------------------

        /// <summary>
        /// Retrieves all comments associated with a specific post.
        /// </summary>

        public async Task<List<CommentResponseDto>> GetByPostAsync(int postId)
        {
            var comments = await _repo.GetCommentsByPostAsync(postId);

            return comments.Select(c => new CommentResponseDto
            {
                CommentID = c.CommentID,
                Text = c.Text,
                PostID = c.PostID,
                UserID = c.UserID
            }).ToList();
        }

        // -------------------- ADD COMMENT --------------------

        /// <summary>
        /// Adds a new comment to a post.
        /// Enforces business rules such as:
        /// - The post must exist.
        /// - A user cannot comment on their own post.
        /// </summary>

        public async Task AddAsync(CreateCommentDto dto, int userId)
        {
            // Fetch post by ID (efficient database query)
            var targetPost = await _postRepo.GetByIdAsync(dto.PostID);

            if (targetPost == null)
                throw new ArgumentException("Post not found.");

            // Business rule: users cannot comment on their own posts
            if (targetPost.UserID == userId)
                throw new InvalidOperationException("Cannot comment your own post.");

            // Map DTO to Comment entity
            var comment = new Comment
            {
                Text = dto.Text,
                PostID = dto.PostID,
                UserID = userId
            };

            await _repo.AddCommentAsync(comment);
        }

        // -------------------- DELETE COMMENT --------------------

        /// <summary>
        /// Deletes a comment.
        /// Only the user who created the comment is allowed to delete it.
        /// </summary>

        public async Task<bool> DeleteAsync(int commentId, int userId)
        {
            return await _repo.DeleteCommentAsync(commentId, userId);
        }
    }
}