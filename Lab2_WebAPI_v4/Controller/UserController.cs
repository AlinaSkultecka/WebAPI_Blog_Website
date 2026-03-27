using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_WebAPI_v4.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly BlobLoggingService _logger;

        public UserController(IUserService service, BlobLoggingService logger)
        {
            _service = service;
            _logger = logger;
        }

        // -------------------- GET ALL USERS --------------------

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllAsync();

            await _logger.LogAsync("All users retrieved");

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
                return NotFound();

            await _logger.LogAsync($"User updated: {dto.UserID}");

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
                return NotFound();

            await _logger.LogAsync($"User deleted: {id}");

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
                return Unauthorized();
            }

            await _logger.LogAsync($"User logged in: {request.UserName}");

            return Ok(new { Token = token });
        }
    }
}