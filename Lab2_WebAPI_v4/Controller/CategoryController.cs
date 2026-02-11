using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lab2_WebAPI_v4.Controller
{
    /// <summary>
    /// Handles operations related to blog post categories.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        /// <summary>
        /// Constructor – injects CategoryService via dependency injection.
        /// </summary>
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // -------------------- GET ALL CATEGORIES --------------------

        /// <summary>
        /// Returns all available categories.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all categories",
            Description = "Returns a list of all available blog post categories."
        )]
        [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        // -------------------- GET CATEGORY POST COUNTS --------------------

        /// <summary>
        /// Returns all categories including number of posts in each category.
        /// </summary>
        [HttpGet("counts")]
        [SwaggerOperation(
            Summary = "Get category post counts",
            Description = "Returns all categories with number of posts per category."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCounts()
        {
            var counts = await _service.GetCountsAsync();
            return Ok(counts);
        }

        // -------------------- ADD CATEGORY --------------------

        /// <summary>
        /// Creates a new category.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create new category",
            Description = "Creates a new blog post category."
        )]
        [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            try
            {
                await _service.AddCategoryAsync(category);

                return CreatedAtAction(
                    nameof(GetAll),
                    new { },
                    category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


