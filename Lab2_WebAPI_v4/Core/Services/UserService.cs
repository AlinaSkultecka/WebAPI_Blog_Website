using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Lab2_WebAPI_v4.DTOs.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab2_WebAPI_v4.Core.Services
{
    /// <summary>
    /// Handles business logic related to users (CRUD + Authentication).
    /// Communicates with repository layer and handles JWT creation.
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
        /// Retrieves all users.
        /// </summary>
        public async Task<List<User>> GetAllAsync()
        {
            return await _repo.GetAllUsersAsync();
        }

        // -------------------- CREATE USER --------------------

        /// <summary>
        /// Creates a new user.
        /// Password hashing is handled in repository.
        /// </summary>
        public async Task AddAsync(User user)
        {
            await _repo.AddUserAsync(user);
        }

        // -------------------- UPDATE USER --------------------

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <returns>True if user exists and was updated.</returns>
        public async Task<bool> UpdateAsync(User user)
        {
            var existingUsers = await _repo.GetAllUsersAsync();

            if (!existingUsers.Any(u => u.UserID == user.UserID))
                return false;

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
            var users = await _repo.GetAllUsersAsync();

            if (!users.Any(u => u.UserID == id))
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

            // Generate JWT token
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


