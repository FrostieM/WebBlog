using System.Linq;
using WebBlog.Model.Interfaces.Database;

namespace WebBlog.Model.EfData
{
    public class EFBlogRepository : IBlogRepository
    {
        
        public IQueryable<Blog> Blogs { get; }
        public void SaveBlog(Blog blog)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteBlog(Blog blog)
        {
            throw new System.NotImplementedException();
        }
    }
}