using System;
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
            comment.Created = DateTime.Now;
            
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        //Delete comment but not a branch
        public void DeleteComment(Comment comment)
        {
            if (!Comments.Any(c => c.ParentComment.Equals(comment)))
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