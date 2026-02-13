using Lab2_WebAPI_v4.Data.DTOs.User;
using Lab2_WebAPI_v4.Data.Entities;

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
