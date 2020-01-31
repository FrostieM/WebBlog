using System;
using System.IO;
using System.Linq;
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
        public IActionResult GetPosts([FromBody] PostsInfo postsInfo)
        {
            var blog = _blogRepository.Blogs
                .FirstOrDefault(b => b.User.UserName == postsInfo.Username);
            
            if (blog == null) 
                return NotFound("User not found");

            var tagsIsNotExist = postsInfo.Tags == null || !postsInfo.Tags.Any();
            var posts = _postRepository.Posts.Include(p => p.PostTags)
                .Where(p => (postsInfo.Type == null || p.Type == postsInfo.Type) &&
                            p.Blog == blog && 
                            (tagsIsNotExist || p.PostTags.Any(t => postsInfo.Tags.Contains(t.Tag.Name))))
                .OrderByDescending(p => p.Created);

            var totalItems = posts.Count();
            
            return Ok(new UserPostsViewData
            {
                Posts = posts
                    .Skip((postsInfo.CurrentPage - 1) * ItemsPerPage)
                    .Take(ItemsPerPage)
                    .Select(p => new ItemViewData<Post>
                    {
                        Item = p,
                        Likes = p.Likes.Count,
                        IsLiked = _postLikeRepository.IsLiked(User.Identity.Name, p.Id),
                        Comments = p.Comments.Count
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
        public IActionResult SavePost([FromForm]PostForm postForm)
        {
            var blog = _blogRepository.Blogs.FirstOrDefault(b => b.User.UserName == User.Identity.Name);
            if (blog == null) return NotFound("User not found");

            string fileUrl = null;
            if (postForm.Type == "image" || postForm.Type == "video")
            {
                fileUrl = AddFile(postForm.File, User.Identity.Name);
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
        public IActionResult DeletePost(int postId)
        {
        
            var blog = _blogRepository.Blogs
                .Include(b => b.Posts)
                .FirstOrDefault(b => b.User.UserName == User.Identity.Name);
            
            if (blog == null) 
                return NotFound("User not found");

            var post = blog.Posts.FirstOrDefault(p=> p.Id == postId);
            
            if (post == null) return NotFound("Post not found");
            
            _postRepository.DeletePost(post);
            return Ok("deleted");
        }

        private static string AddFile(IFormFile file, string username)
        {
            if (file.Length <= 0) return null;
            
            var fileType = file.ContentType.Split("/");
            
            var userDir = Path.Combine("Resources", username);
            var fileTypeDir = Path.Combine(userDir, fileType[0]);
            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), fileTypeDir);
            
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            var fileName = Guid.NewGuid().ToString() + "." + fileType[1];
            
            var fullPath = Path.Combine(dirPath, fileName);
            var dbPath = Path.Combine(fileTypeDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return dbPath;
        }
    }
}