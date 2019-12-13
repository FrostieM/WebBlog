using System.Linq;
using WebBlog.Model.Interfaces.Repository;

namespace WebBlog.Model.EfData
{
    public class EFCommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Comment> Comments => _context.Comments;
        
        public void SaveComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }
    }
}