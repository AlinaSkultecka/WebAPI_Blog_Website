using Lab2_WebAPI_v4.DTOs;
using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Data.Interfaces
{
    public interface ICommentRepo
    {
        Task<List<Comment>> GetCommentsByPostAsync(int postId);
        Task AddCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int commentId, int userId);
    }
}

