namespace Lab2_WebAPI_v4.DTOs.Post
{
    public class UpdatePostDto
    {
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int CategoryID { get; set; }
    }
}
