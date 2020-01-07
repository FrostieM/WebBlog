﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Model;
using WebBlog.Model.Forms;
using WebBlog.Model.Interfaces.Repositories;
using WebBlog.Model.ViewData;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IBlogRepository _blogRepository;

        public int ItemsPerPage { get; set; } = 9;

        public PostsController(
            IPostRepository postRepository,
            IPostLikeRepository postLikeRepository,
            IBlogRepository blogRepository)
        {
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _blogRepository = blogRepository;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post([FromBody] PostsInfo postsInfo)
        {
            var blog = _blogRepository.Blogs
                .FirstOrDefault(b => b.User.UserName == postsInfo.Username);
            
            if (blog == null) 
                return NotFound("User not found");

            var tagsIsNotExist = postsInfo.Tags == null || !postsInfo.Tags.Any();
            var posts = _postRepository.Posts.Include(p => p.PostTags)
                .Where(p => p.Type == postsInfo.Type && p.Blog == blog && 
                            (tagsIsNotExist || p.PostTags.Any(t => postsInfo.Tags.Contains(t.Tag.Name))))
                .OrderByDescending(p => p.Created);

            var totalItems = posts.Count();
            
            return Ok(new UserPostsViewData
            {
                Posts = posts
                    .Skip((postsInfo.CurrentPage - 1) * ItemsPerPage)
                    .Take(ItemsPerPage)
                    .Select(p => new PostViewData
                    {
                        Post = p,
                        Likes = p.Likes.Count,
                        IsLiked = _postLikeRepository.IsLiked(User.Identity.Name, p.Id)
                    }),
                
                PagingInfo = new PagingInfo
                {
                    CurrentPage = postsInfo.CurrentPage,
                    TotalItems = totalItems,
                    ItemsPerPage = ItemsPerPage
                }
            });
        }

        [Authorize]
        [HttpPost, Route("savePost")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromForm]PostForm postForm)
        {
            var blog = _blogRepository.Blogs.FirstOrDefault(b => b.User.UserName == User.Identity.Name);
            if (blog == null) return NotFound("User not found");

            var fileUrl = "";
            if (postForm.Type == "image" || postForm.Type == "video")
            {
                fileUrl = AddFile(postForm.File);
                if (fileUrl == null) return StatusCode(500, "File upload failed");
            }
            
            var post = new Post
            {
                Blog = blog,
                Description = postForm.Description,
                Title = postForm.Title,
                Type = postForm.Type,
                FileUrl = fileUrl,
                Created = DateTime.Now,
            };
            _postRepository.SavePost(post, postForm.Tags.Split(" "));
            
            return Ok("saved");
        }

        [Authorize]
        [HttpGet, Route("deletePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePost(int id)
        {
            var blog = _blogRepository.Blogs
                .Include(b => b.Posts)
                .FirstOrDefault(b => b.User.UserName == User.Identity.Name);
            
            if (blog == null) 
                return NotFound("User not found");

            var post = blog.Posts.FirstOrDefault(p=> p.Id == id);
            
            if (post == null) return NotFound("Post not found");
            
            _postRepository.DeletePost(post);
            return Ok("deleted");
        }

        private static string AddFile(IFormFile file)
        {
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (file.Length <= 0) return null;
            
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return dbPath;
        }
    }
}