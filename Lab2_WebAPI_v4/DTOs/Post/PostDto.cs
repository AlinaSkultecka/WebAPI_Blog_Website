namespace Lab2_WebAPI_v4.DTOs.Post
{
    public class PostDto
    {
        public int PostID { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
    }
}

