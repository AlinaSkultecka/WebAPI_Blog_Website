using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Data.Interfaces
{
    public interface IPostRepo
    {
        List<Post> GetAllPosts();
        void AddPost(Post post);
        bool UpdatePost(Post post);
        bool DeletePost(int postId, int userId);
    }
}

