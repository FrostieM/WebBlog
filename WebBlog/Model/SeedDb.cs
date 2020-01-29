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

            var tag = new Tag {Name = "test"};
            var user = new User
            {
                UserName = "admin",
                Password = "superPassword",
                Email = "admin@email.com",
                FirstName = "Max",
                LastName = "Osinov",
                ImageUrl = "Resources/_shared/image/anonymous.png"
            };
            context.Users.Add(user);
                
            var blog = new Blog
            {
                User = user,
                CreatedDate = DateTime.Now
            };
            context.Blogs.Add(blog);

            for (var i = 0; i < 100; i++)
            {
                var post = new Post 
                {
                    Blog = blog,
                    Created = DateTime.Now,
                    Description = $"test post's description{i+1}",
                    Title = $"test post's title{i+1}",
                    Type = "article"
                };
                context.Posts.Add(post);
                            
                var postTags = new PostTags{Post = post, Tag = tag};
                context.PostTags.Add(postTags);
            }
            context.SaveChanges();
        }
    }
}