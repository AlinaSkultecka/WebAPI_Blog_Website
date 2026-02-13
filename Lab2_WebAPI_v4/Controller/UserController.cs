using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_WebAPI_v4.Controller
{
    /// <summary>
    /// API controller responsible for user management and authentication.
    /// Handles CRUD operations and login functionality.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserController(IUserService service)
        {
            _service = service;
        }

        // -------------------- GET ALL USERS --------------------

        /// <summary>
        /// Retrieves all registered users.
        /// Requires authentication.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        // -------------------- CREATE USER --------------------

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.AddAsync(dto);

            return StatusCode(StatusCodes.Status201Created);
        }

        // -------------------- UPDATE USER --------------------

        /// <summary>
        /// Updates an existing user's information.
        /// Requires authentication.
        /// </summary>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // -------------------- DELETE USER --------------------

        /// <summary>
        /// Deletes a user by ID.
        /// Requires authentication.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // -------------------- LOGIN --------------------

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
       
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _service.LoginAsync(request);

            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}
