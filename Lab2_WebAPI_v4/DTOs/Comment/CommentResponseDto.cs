namespace Lab2_WebAPI_v4.DTOs.Comment
{
    public class CommentResponseDto
    {
        public int CommentID { get; set; }
        public string Text { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
    }
}

