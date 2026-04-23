using Lab2_WebAPI_v4;
using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_WebAPI_v4.Controller
{
    [ApiController]
    [Route("api/[controller]")]

    // This controller manages user-related operations, including registration, authentication, and user management.
    public class UserController : ControllerBase
    {
        // Dependencies for user service, blob logging, and application logging.
        private readonly IUserService _service;
        private readonly BlobLoggingService _logger;
        private readonly ILogger<UserController> _appLogger;

        // Constructor with dependency injection for the user service, blob logging, and application logging.
        public UserController(
            IUserService service,
            BlobLoggingService logger,
            ILogger<UserController> appLogger)
        {
            _service = service;
            _logger = logger;
            _appLogger = appLogger;
        }

        // -------------------- GET ALL USERS --------------------
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllAsync();

            await _logger.LogAsync("All users retrieved");
            _appLogger.LogInformation("All users retrieved.");

            return Ok(users);
        }

        // -------------------- CREATE USER --------------------
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.AddAsync(dto);

            await _logger.LogAsync($"New user registered: {dto.UserName}");
            _appLogger.LogInformation("New user registered: {UserName}", dto.UserName);

            return StatusCode(StatusCodes.Status201Created);
        }

        // -------------------- UPDATE USER --------------------
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(dto);

            if (!updated)
            {
                _appLogger.LogWarning("User update failed. User {UserId} not found.", dto.UserID);
                return NotFound();
            }

            await _logger.LogAsync($"User updated: {dto.UserID}");
            _appLogger.LogInformation("User updated: {UserId}", dto.UserID);

            return NoContent();
        }

        // -------------------- DELETE USER --------------------
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                _appLogger.LogWarning("User delete failed. User {UserId} not found.", id);
                return NotFound();
            }

            await _logger.LogAsync($"User deleted: {id}");
            _appLogger.LogInformation("User deleted: {UserId}", id);

            return NoContent();
        }

        // -------------------- LOGIN --------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _service.LoginAsync(request);

            if (token == null)
            {
                await _logger.LogAsync($"Failed login attempt for username: {request.UserName}");
                _appLogger.LogWarning("Failed login attempt for username: {UserName}", request.UserName);
                return Unauthorized();
            }

            await _logger.LogAsync($"User logged in: {request.UserName}");
            _appLogger.LogInformation("User logged in: {UserName}", request.UserName);

            return Ok(new { Token = token });
        }
    }
}