using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Lab2_WebAPI_v4.DTOs.Category;

namespace Lab2_WebAPI_v4.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _repo;

        public CategoryService(ICategoryRepo repo)
        {
            _repo = repo;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _repo.GetAllCategoriesAsync();
        }

        public async Task<List<CategoryCountDto>> GetCountsAsync()
        {
            return await _repo.GetCategoryCountsAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name is required");

            await _repo.AddCategoryAsync(category);
        }
    }
}
