using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        // Navigation properties should NOT be required for POST
        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }

        [JsonIgnore]
        public List<Comment>? Comments { get; set; } = new();
    }
}

