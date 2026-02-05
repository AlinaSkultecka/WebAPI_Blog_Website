using System.ComponentModel.DataAnnotations;

namespace Lab2_WebAPI_v4.Data.Entities
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        // Foreign keys
        [Required]
        public int UserID { get; set; }
        
        [Required]
        public int CategoryID { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

