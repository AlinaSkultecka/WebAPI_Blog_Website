using Lab2_WebAPI_v4.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_WebAPI_v4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _repo;

        public CategoryController(ICategoryRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAllCategories());
        }

        [HttpGet("counts")]
        public IActionResult GetCounts()
        {
            return Ok(_repo.GetCategoryCounts());
        }
    }
}
