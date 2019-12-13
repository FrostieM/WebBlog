using System.Linq;

namespace WebBlog.Model.Interfaces.Repository
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }

        void SavePost(Post post);
        void DeletePost(Post post);
    }
}