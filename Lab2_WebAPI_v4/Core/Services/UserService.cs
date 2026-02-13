using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.DTOs.User;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab2_WebAPI_v4.Core.Services
{
    /// <summary>
    /// Handles business logic related to users (CRUD + Authentication).
    /// Responsible for validation and JWT token generation.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepo _repo;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepo repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        // -------------------- GET ALL USERS --------------------

        /// <summary>
        /// Retrieves all registered users.
        /// </summary>
        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _repo.GetAllUsersAsync();

            return users.Select(u => new UserResponseDto
            {
                UserID = u.UserID,
                Email = u.Email,
                UserName = u.UserName
            }).ToList();
        }

        // -------------------- CREATE USER --------------------

        /// <summary>
        /// Creates a new user account.
        /// Password is stored as provided (no hashing implemented).
        /// </summary>
        public async Task AddAsync(CreateUserDto dto)
        {
            var user = new User
            {
                Email = dto.Email,
                Password = dto.Password
            };

            await _repo.AddUserAsync(user);
        }

        // -------------------- UPDATE USER --------------------

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <returns>True if user exists and was updated.</returns>
        public async Task<bool> UpdateAsync(UpdateUserDto dto)
        {
            var user = await _repo.GetByIdAsync(dto.UserID);

            if (user == null)
                return false;

            user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.Password = dto.Password;
            }

            await _repo.UpdateUserAsync(user);
            return true;
        }

        // -------------------- DELETE USER --------------------

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <returns>True if user existed and was deleted.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user == null)
                return false;

            await _repo.DeleteUserAsync(id);
            return true;
        }

        // -------------------- LOGIN --------------------

        /// <summary>
        /// Validates user credentials and generates JWT token if valid.
        /// </summary>
        /// <returns>JWT token string or null if authentication fails.</returns>
        public async Task<string?> LoginAsync(LoginRequestDto request)
        {
            var userId = await _repo.LoginAsync(request.UserName, request.Password);

            if (userId == -1)
                return null;

            // Create claims for authenticated user
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim("UserID", userId.ToString())
            };

            var jwtSettings = _configuration.GetSection("Jwt");

            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var signinCredentials =
                new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}