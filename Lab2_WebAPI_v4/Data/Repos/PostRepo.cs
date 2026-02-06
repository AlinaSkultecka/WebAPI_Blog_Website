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
            if (post.PostID != 0)
                throw new ArgumentException("PostID must be 0 when creating a new post.");

            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public bool UpdatePost(Post post)
        {
            var postOrg = _context.Posts
                .SingleOrDefault(p => p.PostID == post.PostID && p.UserID == post.UserID);

            if (postOrg == null)
                return false;

            // update only the fields that are allowed to change
            postOrg.Title = post.Title;
            postOrg.Text = post.Text;
            postOrg.CategoryID = post.CategoryID;

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

