using System.Linq;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface IBlogRepository
    {
        IQueryable<Blog> Blogs { get; }
        
        void SaveBlog(Blog blog);
        void DeleteBlog(Blog blog);
    }
}