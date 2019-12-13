using System.Linq;
using WebBlog.Model.Interfaces.Repository;

namespace WebBlog.Model.EfData
{
    public class EFPostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public EFPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts => _context.Posts;
        
        public void SavePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void DeletePost(Post post)
        {
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }
    }
}