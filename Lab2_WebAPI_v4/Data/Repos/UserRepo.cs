using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab2_WebAPI_v4.Data.Repos
{
    /// <summary>
    /// Repository responsible for database operations related to users.
    /// Handles CRUD operations and authentication lookup.
    /// </summary>
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor – injects the application's DbContext.
        /// </summary>
        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        // -------------------- CREATE USER --------------------

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // -------------------- DELETE USER --------------------

        /// <summary>
        /// Deletes a user by ID.
        /// If user does not exist, method exits silently.
        /// </summary>
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(p => p.UserID == id);

            if (user == null)
                return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        // -------------------- GET ALL USERS --------------------

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>List of User entities.</returns>
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // -------------------- UPDATE USER --------------------

        /// <summary>
        /// Updates an existing user.
        /// Uses EF tracking to update current values.
        /// </summary>
        public async Task UpdateUserAsync(User userUpdated)
        {
            var userOrg = await _context.Users
                .SingleOrDefaultAsync(p => p.UserID == userUpdated.UserID);

            if (userOrg == null)
                return;

            // Copy updated values into tracked entity
            _context.Entry(userOrg).CurrentValues.SetValues(userUpdated);

            await _context.SaveChangesAsync();
        }

        // -------------------- LOGIN --------------------

        /// <summary>
        /// Validates user credentials.
        /// Returns UserID if valid, otherwise -1.
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password (plain text for lab purposes)</param>
        /// <returns>UserID or -1 if authentication fails</returns>
        public async Task<int> LoginAsync(string userName, string password)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u =>
                    u.UserName == userName &&
                    u.Password == password);

            if (user == null)
                return -1;

            return user.UserID;
        }
    }
}
