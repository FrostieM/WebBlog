using System.Linq;

namespace WebBlog.Model.Interfaces.Repository
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        void SaveComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}