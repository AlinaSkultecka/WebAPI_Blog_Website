using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Data.Interfaces
{
    public interface IPostRepo
    {
        Task<List<Post>> GetAllPostsAsync();
        Task AddPostAsync(Post post);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int postId, int userId);
        Task<List<Post>> SearchByTitleAsync(string title);
        Task<List<Post>> SearchByCategoryAsync(int categoryId);

    }
}

