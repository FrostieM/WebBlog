using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebBlog.Model.Interfaces.Repositories;

namespace WebBlog.Model.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts => _context.Posts;
        
        public void SavePost(Post post, IEnumerable<string> rowTags)
        {
            _context.Posts.Add(post);
            
            foreach (var tag in rowTags)
            {
               var currentTag = _context.Tags.FirstOrDefault(t => t.Name == tag) ?? new Tag{Name = tag};
               _context.PostTags.Add(new PostTags {Post = post, Tag = currentTag});
            }
            
            _context.SaveChanges();
        }

        public void DeletePost(Post post)
        {
            if (post.FileUrl != null)
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), post.FileUrl));
            }
            
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }
    }
}