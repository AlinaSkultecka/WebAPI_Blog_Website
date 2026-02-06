using Lab2_WebAPI_v4.Data.DTOs;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;

namespace Lab2_WebAPI_v4.Data.Repos
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public List<CategoryCountDto> GetCategoryCounts()
        {
            return _context.Categories
                .Select(c => new CategoryCountDto
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    PostCount = _context.Posts.Count(p => p.CategoryID == c.CategoryID)
                })
                .ToList();
        }
    }
}
