using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebBlog.Model
{
    public static class SeedDb
    {
        public static void CreateConnection(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            if (context.Users.Any()) return;
            
            var user = new User
            {
                UserName = "admin",
                Password = "superPassword",
                Email = "admin@email.com",
                FirstName = "Max",
                LastName = "Os"
            };
            context.Users.Add(user);
                
            var blog = new Blog
            {
                User = user,
                CreatedDate = DateTime.Now
            };
            context.Blogs.Add(blog);
            
            var post = new Post
            {
                Blog = blog,
                Created = DateTime.Now,
                Description = "test post's description",
                Title = "test post's title",
                Type = "article"
            };
            context.Posts.Add(post);
            
            var postTags = new PostTags{Post = post, Tag = new Tag{Name = "test"}};
            context.PostTags.Add(postTags);
            
            context.SaveChanges();
        }
    }
}