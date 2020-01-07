using System.Collections.Generic;
using System.Linq;
using WebBlog.Model.ViewData;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags { get; }

        void SaveTag(Tag tag);
        void DeleteTag(Tag tag);

        IEnumerable<TagViewData> GetBlogTags(Blog blog, string type);
    }
}