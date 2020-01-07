using System.Linq;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface ICommentLikeRepository
    {
        IQueryable<CommentLike> CommentLikes { get; }

        void SaveCommentLike(CommentLike commentLike);
        void DeleteCommentLike(CommentLike commentLike);
    }
}