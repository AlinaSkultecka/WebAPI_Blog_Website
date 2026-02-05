using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Data.Interfaces
{
    public interface IUserRepo
    {
        List<User> GetAllUsers();

        void AddUser(User user);

        void UpdateUser(User user);
        void DeleteUser(int id);
        int Login(string userName, string password);

    }
}
