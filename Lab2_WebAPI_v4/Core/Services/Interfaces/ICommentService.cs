using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Core.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetByPostAsync(int postId);
        Task AddAsync(Comment comment, int userId);
        Task<bool> DeleteAsync(int commentId, int userId);
    }
}

