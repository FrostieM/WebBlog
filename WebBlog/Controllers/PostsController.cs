using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserRepository _userRepository;

        public int ItemsPerPage { get; set; } = 8;

        public PostsController(
            IPostRepository postRepository,
            IPostLikeRepository postLikeRepository,
            IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet, Route("{type}/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string type, string username, int currentPage = 1)
        {
            var userExist = _userRepository.Users.Any(u => u.UserName == username);
            if (!userExist) 
                return NotFound("User not found");
            
            var userPosts = _postRepository.Posts
                .Where(p => 
                    p.Blog.User.UserName == username && 
                    p.Type == type)
                .Select(p => new PostViewData
                {
                    Post = p,
                    Likes = _postLikeRepository.getLikes(p),
                    IsLiked = _postLikeRepository.isLiked(User.Identity.Name)
                });

            var totalItems = userPosts.Count();
            
            return Ok(new UserPostsViewData
            {
                Posts = userPosts
                    .Skip((currentPage - 1) * ItemsPerPage)
                    .Take(ItemsPerPage),
                
                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    TotalItems = totalItems,
                    ItemsPerPage = ItemsPerPage
                }
            });
        }

        [Authorize]
        [HttpPost, Route("savePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post([FromBody]PostForm postForm)
        {
            if (!_userRepository.Users.Any(u => u.UserName == User.Identity.Name)) 
                return NotFound("User not found");
            
            var post = new Post
            {
                Description = postForm.Description,
                Title = postForm.Title,
                Type = postForm.Type,
                FileUrl = postForm.FileUrl,
                Created = DateTime.Now.ToString(CultureInfo.CurrentCulture),
            };
            _postRepository.SavePost(post, User.Identity.Name);
            
            return Ok(Get(postForm.Type, User.Identity.Name));
        }
    }
}