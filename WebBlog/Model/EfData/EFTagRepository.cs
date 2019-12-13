using System.Linq;
using WebBlog.Model.Interfaces.Repository;

namespace WebBlog.Model.EfData
{
    public class EFTagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public EFTagRepository(ApplicationDbContext context)
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
    }
}