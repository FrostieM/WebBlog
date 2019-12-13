using System.Linq;

namespace WebBlog.Model.Interfaces.Repository
{
    public interface IBlogRepository
    {
        IQueryable<Blog> Blogs { get; }
        
        void SaveBlog(Blog blog);
        void DeleteBlog(Blog blog);
    }
}