using Lab2_WebAPI_v4.Data.DTOs;
using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Data.Interfaces
{
    public interface ICommentRepo
    {
        List<Comment> GetCommentsByPost(int postId);
        void AddComment(Comment comment, int userId);
        bool DeleteComment(int commentId, int userId);
    }
}

