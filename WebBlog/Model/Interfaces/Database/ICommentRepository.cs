using System.Linq;

namespace WebBlog.Model.Interfaces.Database
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        void SaveComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}