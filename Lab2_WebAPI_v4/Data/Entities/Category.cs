using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lab2_WebAPI_v4.Data.Entities
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        // Navigation property
        [JsonIgnore]
        public List<Post> Posts { get; set; }
    }
}
