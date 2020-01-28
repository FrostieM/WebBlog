using System.Linq;
using WebBlog.Model.Interfaces.Repositories;

namespace WebBlog.Model.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
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
            if (!comment.SubComments.Any())
            {
                _context.Comments.Remove(comment);
            }
            else
            {
                comment.Content = null;
            }
            
            _context.SaveChanges();
        }
    }
}