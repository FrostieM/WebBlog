using System.Linq;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface IPostLikeRepository: ILikeAble
    {
        IQueryable<PostLike> PostLikes { get; }
        
        void SavePostLike(PostLike postLike);
        void DeletePostLike(PostLike postLike);
        
        
    }
}