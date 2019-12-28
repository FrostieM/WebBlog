using System.Linq;
using WebBlog.Model.Interfaces.Repositories;

namespace WebBlog.Model.Repositories
{
    public class CommentLikeRepository : ICommentLikeRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentLikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<CommentLike> CommentLikes => _context.CommentLikes;
        
        public void SaveCommentLike(CommentLike commentLike)
        {
            _context.CommentLikes.Add(commentLike);
            _context.SaveChanges();
        }

        public void DeleteCommentLike(CommentLike commentLike)
        {
            _context.CommentLikes.Remove(commentLike);
            _context.SaveChanges();
        }
    }
}