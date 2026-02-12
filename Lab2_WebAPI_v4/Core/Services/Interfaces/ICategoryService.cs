using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.DTOs.Category;

namespace Lab2_WebAPI_v4.Core.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllAsync();
        Task<List<CategoryCountDto>> GetCountsAsync();
        Task AddCategoryAsync(CreateCategoryDto dto);
    }
}

