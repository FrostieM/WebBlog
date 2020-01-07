using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }

        void SavePost(Post post, IEnumerable<string> rowTags);
        void DeletePost(Post post);
    }
}