using System.ComponentModel.DataAnnotations;

namespace Lab2_WebAPI_v4.Data.Entities
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        // Foreign keys
        [Required]
        public int UserID { get; set; }

        [Required]
        public int PostID { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
