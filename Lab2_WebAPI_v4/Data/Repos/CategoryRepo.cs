using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Lab2_WebAPI_v4.DTOs.Category;
using Microsoft.EntityFrameworkCore;

namespace Lab2_WebAPI_v4.Data.Repos
{
    /// <summary>
    /// Repository responsible for all database operations related to categories.
    /// Communicates directly with the DbContext (Entity Framework).
    /// </summary>
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor – injects the application's DbContext.
        /// </summary>
        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }

        // -------------------- GET ALL CATEGORIES --------------------

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>List of Category entities.</returns>
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            // Fetch all records from Categories table
            return await _context.Categories.ToListAsync();
        }

        // -------------------- GET CATEGORY POST COUNTS --------------------

        /// <summary>
        /// Retrieves categories along with the number of posts in each category.
        /// </summary>
        /// <returns>List of CategoryCountDto objects.</returns>
        public async Task<List<CategoryCountDto>> GetCategoryCountsAsync()
        {
            /*
             This query:
             - Selects all categories
             - For each category, counts how many posts belong to it
             - Projects the result into a DTO (CategoryCountDto)
             
             DTO is used instead of returning full entities
             because we only need specific data.
            */

            return await _context.Categories
                .Select(c => new CategoryCountDto
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,

                    // Count posts linked to this category
                    PostCount = _context.Posts
                        .Count(p => p.CategoryID == c.CategoryID)
                })
                .ToListAsync();
        }

        // -------------------- ADD CATEGORY --------------------

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        public async Task AddCategoryAsync(Category category)
        {
            // Add new category entity
            await _context.Categories.AddAsync(category);

            // Persist changes to database
            await _context.SaveChangesAsync();
        }
    }
}
