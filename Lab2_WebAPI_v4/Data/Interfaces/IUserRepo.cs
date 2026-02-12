using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Data.Interfaces
{
    public interface IUserRepo
    {
        Task<List<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<int> LoginAsync(string userName, string password);
        Task<User?> GetByIdAsync(int id);
    }
}
