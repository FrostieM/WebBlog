﻿using System.Linq;
using WebBlog.Model.Interfaces.Repositories;

namespace WebBlog.Model.Repositories
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly ApplicationDbContext _context;

        public PostLikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<PostLike> PostLikes => _context.PostLikes;
        
        public void SavePostLikes(PostLike postLike)
        {
            _context.PostLikes.Add(postLike);
            _context.SaveChanges();
        }

        public void DeletePostLikes(PostLike postLike)
        {
            _context.PostLikes.Remove(postLike);
            _context.SaveChanges();
        }

        public int getLikes(Post post)
        {
            return PostLikes.Count(l => l.Post.Equals(post));
        }

        public bool isLiked(string username)
        {
            return PostLikes.Any(l => l.User.UserName.Equals(username));
        }
    }
}