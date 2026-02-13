using Lab2_WebAPI_v4.Data.DTOs.Comment;
using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Core.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentResponseDto>> GetByPostAsync(int postId);
        Task AddAsync(CreateCommentDto dto, int userId);
        Task<bool> DeleteAsync(int commentId, int userId);
    }
}

