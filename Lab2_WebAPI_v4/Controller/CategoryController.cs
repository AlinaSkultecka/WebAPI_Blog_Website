using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lab2_WebAPI_v4.Controller
{
    /// <summary>
    /// API controller responsible for handling operations related to blog post categories.
    /// Provides endpoints for retrieving categories and creating new ones.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // -------------------- GET ALL CATEGORIES --------------------

        /// <summary>
        /// Retrieves all available blog post categories.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all categories",
            Description = "Returns a list of all available blog post categories."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();

            // Returns HTTP 200 with category list
            return Ok(categories);
        }

        // -------------------- GET CATEGORY POST COUNTS --------------------

        /// <summary>
        /// Retrieves all categories including the number of posts in each category.
        /// </summary>
        [HttpGet("counts")]
        [SwaggerOperation(
            Summary = "Get category post counts",
            Description = "Returns all categories with the number of posts per category."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCounts()
        {
            var counts = await _service.GetCountsAsync();

            // Returns HTTP 200 with category counts
            return Ok(counts);
        }

        // -------------------- ADD CATEGORY --------------------

        /// <summary>
        /// Creates a new blog post category.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create new category",
            Description = "Creates a new blog post category."
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateCategoryDto dto)
        {
            // Automatic model validation (thanks to [ApiController])
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.AddCategoryAsync(dto);

                // Returns HTTP 201 (Created)
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (ArgumentException ex)
            {
                // Returns 400 if business validation fails
                return BadRequest(ex.Message);
            }
        }
    }
}