using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.DTOs.Category;

namespace Lab2_WebAPI_v4.Data.Interfaces
{
    public interface ICategoryRepo
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<CategoryCountDto>> GetCategoryCountsAsync();
        Task AddCategoryAsync(Category category);
        Task<bool> CategoryExistsAsync(int categoryId);
    }
}