using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.DTOs.User;

namespace Lab2_WebAPI_v4.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllAsync();
        Task AddAsync(CreateUserDto dto);
        Task<bool> UpdateAsync(UpdateUserDto dto);
        Task<bool> DeleteAsync(int id);
        Task<string?> LoginAsync(LoginRequestDto request);
    }
}
