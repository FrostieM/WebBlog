using System.Linq;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface IPostLikeRepository
    {
        IQueryable<PostLike> PostLikes { get; }
        
        void SavePostLikes(PostLike postLike);
        void DeletePostLikes(PostLike postLike);
        
        int GetLikes(int postId);
        bool IsLiked(string username, int postId);
    }
}