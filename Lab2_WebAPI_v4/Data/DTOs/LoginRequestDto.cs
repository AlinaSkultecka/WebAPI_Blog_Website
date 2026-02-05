using System.ComponentModel.DataAnnotations;

namespace Lab2_WebAPI_v4.Data.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

