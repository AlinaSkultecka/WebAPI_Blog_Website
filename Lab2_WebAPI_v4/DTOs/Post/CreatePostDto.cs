using System.ComponentModel.DataAnnotations;

namespace Lab2_WebAPI_v4.DTOs.Post
{
    public class CreatePostDto
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        [MinLength(3)]
        public string Text { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CategoryID must be greater than 0.")]
        public int CategoryID { get; set; }
    }
}
