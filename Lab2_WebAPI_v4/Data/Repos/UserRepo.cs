using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;

namespace Lab2_WebAPI_v4.Data.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        //Contexten sätts upp i service containern och injectas till 
        //denna klassen via konstruktorn
        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user= _context.Users.SingleOrDefault(p=> p.UserID==id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void UpdateUser(User userUpdated)
        {
            //Hämta den product som skall uppdateras
            var productOrg = _context.Users
                .SingleOrDefault(p => p.UserID == userUpdated.UserID);

            _context.Entry(productOrg).CurrentValues.SetValues(userUpdated);
            _context.SaveChanges();
        }

        public int Login(string userName, string password)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName && u.Password == password);

            if (user == null)
                return -1;

            return user.UserID;
        }

    }
}
