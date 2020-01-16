using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebBlog.Model.Interfaces.Repositories;
using WebBlog.Model.ViewData;

namespace WebBlog.Model.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> Tags => _context.Tags;
        
        public void SaveTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }

        public void DeleteTag(Tag tag)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }

        public IEnumerable<TagViewData> GetBlogTags(Blog blog, string type)
        {
            return _context.PostTags
                .Where(p => p.Post.Blog == blog && (type==null || p.Post.Type == type))
                .GroupBy(p => p.Tag.Name)
                .Select(t => new TagViewData{Name = t.Key, Count = t.Count()});
        }
    }
}