﻿using System.Linq;
using WebBlog.Model.Interfaces.Repository;

namespace WebBlog.Model.EfData
{
    public class EFBlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public EFBlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Blog> Blogs => _context.Blogs;
        
        public void SaveBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            _context.SaveChanges();
        }

        public void DeleteBlog(Blog blog)
        {
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
        }
    }
}