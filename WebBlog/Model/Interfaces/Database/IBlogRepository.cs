using System.Linq;

namespace WebBlog.Model.Interfaces.Database
{
    public interface IBlogRepository
    {
        IQueryable<Blog> Blogs { get; }
        
        void SaveBlog(Blog blog);
        void DeleteBlog(Blog blog);
    }
}