using System.Linq;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        void SaveComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}