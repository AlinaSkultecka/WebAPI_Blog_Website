using Lab2_WebAPI_v4.Data.DTOs;
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
            var post = _context.Posts.SingleOrDefault(p => p.PostID == comment.PostID);

            if (post == null)
                throw new Exception("Post not found"); // better: custom exception or return false

            if (post.UserID == userId)
                throw new Exception("Cannot comment your own post");

            comment.UserID = userId;
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public bool DeleteComment(int commentId, int userId)
        {
            var comment = _context.Comments
                .SingleOrDefault(c => c.CommentID == commentId && c.UserID == userId);

            if (comment == null)
                return false;

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return true;
        }
    }
}
