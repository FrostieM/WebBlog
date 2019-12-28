using System.Linq;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags { get; }

        void SaveTag(Tag tag);
        void DeleteTag(Tag tag);
    }
}