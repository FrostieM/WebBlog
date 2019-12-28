using System.Linq;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }

        void SavePost(Post post, string username);
        void DeletePost(Post post);
    }
}