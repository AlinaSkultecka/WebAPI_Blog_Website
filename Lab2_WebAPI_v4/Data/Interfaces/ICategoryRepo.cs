using Lab2_WebAPI_v4.Data.DTOs;
using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Data.Interfaces
{
    public interface ICategoryRepo
    {
        List<Category> GetAllCategories();
        List<CategoryCountDto> GetCategoryCounts();
    }
}

