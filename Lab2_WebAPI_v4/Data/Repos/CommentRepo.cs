using Lab2_WebAPI_v4.DTOs;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab2_WebAPI_v4.Data.Repos
{
    /// <summary>
    /// Repository responsible for handling database operations related to comments.
    /// Communicates directly with the DbContext using Entity Framework.
    /// </summary>
    public class CommentRepo : ICommentRepo
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor – injects the application's DbContext.
        /// </summary>
        public CommentRepo(AppDbContext context)
        {
            _context = context;
        }

        // -------------------- GET COMMENTS BY POST --------------------

        /// <summary>
        /// Retrieves all comments belonging to a specific post.
        /// </summary>
        /// <param name="postId">ID of the post</param>
        /// <returns>List of Comment entities</returns>
        public async Task<List<Comment>> GetCommentsByPostAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostID == postId)
                .ToListAsync();
        }

        // -------------------- ADD COMMENT --------------------

        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }


        // -------------------- DELETE COMMENT --------------------

        /// <summary>
        /// Deletes a comment.
        /// Only the comment owner can delete it.
        /// </summary>
        /// <param name="commentId">ID of the comment</param>
        /// <param name="userId">ID of the logged-in user</param>
        /// <returns>True if deleted, otherwise false</returns>
        public async Task<bool> DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _context.Comments
                .SingleOrDefaultAsync(c =>
                    c.CommentID == commentId &&
                    c.UserID == userId);

            if (comment == null)
                return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

