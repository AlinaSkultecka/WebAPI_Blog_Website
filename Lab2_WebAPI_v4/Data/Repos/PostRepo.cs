using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;

namespace Lab2_WebAPI_v4.Data.Repos
{
    public class PostRepo : IPostRepo
    {
        private readonly AppDbContext _context;

        public PostRepo(AppDbContext context)
        {
            _context = context;
        }

        public List<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public bool UpdatePost(Post post)
        {
            var postOrg = _context.Posts
                .SingleOrDefault(p => p.PostID == post.PostID && p.UserID == post.UserID);

            if (postOrg == null)
                return false;

            _context.Entry(postOrg).CurrentValues.SetValues(post);
            _context.SaveChanges();
            return true;
        }

        public bool DeletePost(int postId, int userId)
        {
            var post = _context.Posts
                .SingleOrDefault(p => p.PostID == postId && p.UserID == userId);

            if (post == null)
                return false;

            _context.Posts.Remove(post);
            _context.SaveChanges();
            return true;
        }
    }
}

