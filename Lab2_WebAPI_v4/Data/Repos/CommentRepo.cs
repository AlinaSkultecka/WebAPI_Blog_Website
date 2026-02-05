using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab2_WebAPI_v4.Data.Repos
{
    public class CommentRepo : ICommentRepo
    {
        private readonly AppDbContext _context;

        public CommentRepo(AppDbContext context)
        {
            _context = context;
        }

        public List<Comment> GetCommentsByPost(int postId)
        {
            return _context.Comments
                .Where(c => c.PostID == postId)
                .ToList();
        }

        public void AddComment(Comment comment, int userId)
        {
            // Get post to check ownership
            var post = _context.Posts
                .SingleOrDefault(p => p.PostID == comment.PostID);

            // User cannot comment own post
            if (post.UserID == userId)
                throw new Exception("Cannot comment your own post");

            comment.UserID = userId;

            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(int commentId, int userId)
        {
            var comment = _context.Comments
                .SingleOrDefault(c => c.CommentID == commentId && c.UserID == userId);

            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }
    }
}
