using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.Category;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;

namespace Lab2_WebAPI_v4.Core.Services
{
    /// <summary>
    /// Handles business logic related to blog categories.
    /// Communicates with repository layer and performs validation.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        public CategoryService(ICategoryRepo repo)
        {
            _repo = repo;
        }

        // -------------------- GET ALL CATEGORIES --------------------

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repo.GetAllCategoriesAsync();

            return categories.Select(c => new CategoryResponseDto
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName
            }).ToList();
        }

        // -------------------- GET CATEGORY COUNTS --------------------

        /// <summary>
        /// Retrieves all categories along with the number of posts in each category.
        /// </summary>
        public async Task<List<CategoryCountDto>> GetCountsAsync()
        {
            return await _repo.GetCategoryCountsAsync();
        }

        // -------------------- CREATE CATEGORY --------------------

        /// <summary>
        /// Creates a new category after validating input.
        /// </summary>
        public async Task AddCategoryAsync(CreateCategoryDto dto)
        {
            // Basic null check
            if (dto == null)
                throw new ArgumentException("Request body is required.");

            // Validate category name
            if (string.IsNullOrWhiteSpace(dto.CategoryName))
                throw new ArgumentException("Category name is required.");

            // Create entity from DTO
            var category = new Category
            {
                CategoryName = dto.CategoryName.Trim()
            };

            await _repo.AddCategoryAsync(category);
        }
    }
}

