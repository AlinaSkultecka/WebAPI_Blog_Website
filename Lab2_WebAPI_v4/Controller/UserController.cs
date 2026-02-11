using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_WebAPI_v4.Controller
{
    /// <summary>
    /// Handles user management and authentication (CRUD + Login).
    /// This controller communicates only with the Service layer.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        /// <summary>
        /// Constructor – injects the UserService via dependency injection.
        /// </summary>
        public UserController(IUserService service)
        {
            _service = service;
        }

        // -------------------- GET ALL USERS --------------------

        /// <summary>
        /// Returns all registered users.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        // -------------------- CREATE USER --------------------

        /// <summary>
        /// Creates a new user account.
        /// Password hashing is handled in the service layer.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            await _service.AddAsync(user);
            return Created("", user);
        }

        // -------------------- UPDATE USER --------------------

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var updated = await _service.UpdateAsync(user);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // -------------------- DELETE USER --------------------

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        [HttpDelete("{id}")]
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
        /// Authenticates a user and returns a JWT token if credentials are valid.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var token = await _service.LoginAsync(request);

            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}
