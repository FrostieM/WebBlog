using System.Linq;

namespace WebBlog.Model.Interfaces.Database
{
    public interface ICommentLikeRepository
    {
        IQueryable<CommentLike> CommentLikes { get; }

        void SaveCommentLike(User user, Comment comment);
        void DeleteCommentLike(CommentLike commentLike);
    }
}