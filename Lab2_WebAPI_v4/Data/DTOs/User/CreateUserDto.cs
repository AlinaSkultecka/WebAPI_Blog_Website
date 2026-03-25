namespace Lab2_WebAPI_v4.Data.DTOs.User
{
    public class CreateUserDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
