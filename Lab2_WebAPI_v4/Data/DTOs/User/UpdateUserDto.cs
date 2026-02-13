using System.ComponentModel.DataAnnotations;

namespace Lab2_WebAPI_v4.Data.DTOs.User
{
    /// <summary>
    /// DTO used when updating an existing user.
    /// </summary>
    public class UpdateUserDto
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // user can change password
        public string? Password { get; set; }
    }
}

