using System.Linq;

namespace WebBlog.Model.Interfaces.Repository
{
    public interface IPostLikeRepository
    {
        IQueryable<PostLike> PostLikes { get; }

        void SavePostLikes(PostLike postLike);
        void DeletePostLikes(PostLike postLike);
    }
}