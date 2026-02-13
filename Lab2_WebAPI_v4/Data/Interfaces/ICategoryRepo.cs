using Lab2_WebAPI_v4.Data.DTOs.Category;
using Lab2_WebAPI_v4.Data.Entities;

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