using System.Linq;
using WebBlog.Model.Interfaces.Repositories;

namespace WebBlog.Model.Repositories
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly ApplicationDbContext _context;

        public PostLikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<PostLike> PostLikes => _context.PostLikes;
        
        public void SavePostLikes(PostLike postLike)
        {
            _context.PostLikes.Add(postLike);
            _context.SaveChanges();
        }

        public void DeletePostLikes(PostLike postLike)
        {
            _context.PostLikes.Remove(postLike);
            _context.SaveChanges();
        }

        public int GetLikes(int postId)
        {
            return PostLikes.Count(l => l.Post.Id == postId);
        }

        public bool IsLiked(string username, int postId)
        {
            return PostLikes.Any(l => 
                l.User.UserName.Equals(username) && 
                l.Post.Id == postId);
        }
    }
}