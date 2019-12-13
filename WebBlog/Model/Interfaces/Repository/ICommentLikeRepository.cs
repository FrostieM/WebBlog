using System.Linq;

namespace WebBlog.Model.Interfaces.Repository
{
    public interface ICommentLikeRepository
    {
        IQueryable<CommentLike> CommentLikes { get; }

        void SaveCommentLike(CommentLike commentLike);
        void DeleteCommentLike(CommentLike commentLike);
    }
}