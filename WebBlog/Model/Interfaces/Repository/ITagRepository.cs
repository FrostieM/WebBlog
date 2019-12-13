using System.Linq;

namespace WebBlog.Model.Interfaces.Repository
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags { get; }

        void SaveTag(Tag tag);
        void DeleteTag(Tag tag);
    }
}