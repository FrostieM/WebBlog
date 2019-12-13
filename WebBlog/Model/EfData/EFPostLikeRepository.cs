using System.Linq;
using WebBlog.Model.Interfaces.Repository;

namespace WebBlog.Model.EfData
{
    public class EFPostLikeRepository : IPostLikeRepository
    {
        private readonly ApplicationDbContext _context;

        public EFPostLikeRepository(ApplicationDbContext context)
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
    }
}