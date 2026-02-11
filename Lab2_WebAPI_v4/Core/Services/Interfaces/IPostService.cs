using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.DTOs;
using Lab2_WebAPI_v4.DTOs.Post;

namespace Lab2_WebAPI_v4.Core.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<PostDto>> GetAllAsync();
        Task<PostDto> AddAsync(CreatePostDto dto, int userId);
        Task<bool> UpdateAsync(PostDto dto, int userId);
        Task<bool> DeleteAsync(int postId, int userId);
        Task<List<PostDto>> SearchByTitleAsync(string title);
        Task<List<PostDto>> SearchByCategoryAsync(int categoryId);
    }
}
