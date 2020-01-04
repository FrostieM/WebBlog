using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Model;
using WebBlog.Model.Interfaces.Repositories;
using WebBlog.Model.ViewData;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostLikeController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;

        public PostLikeController(
            IUserRepository userRepository,
            IPostRepository postRepository,
            IPostLikeRepository postLikeRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
        }

        [HttpPost, Route("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post(int postId)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null) return NotFound("User not found");

            var userPost = _postRepository.Posts.FirstOrDefault(p => 
                p.Id == postId);
            
            if (userPost == null) return NotFound("Post not found");

            var postLike = _postLikeRepository.PostLikes
                .FirstOrDefault(l => 
                    l.User.UserName == User.Identity.Name && 
                    l.Post == userPost);

            if (postLike == null) //like is not exist
            {
                _postLikeRepository.SavePostLikes(new PostLike 
                {
                    Post = userPost, 
                    User = _userRepository.Users.First(u => u.UserName == User.Identity.Name)
                });
            }
            else //unlike post
            {
                _postLikeRepository.DeletePostLikes(postLike);
            }

            return Ok(new PostViewData
            {
                Post = userPost,
                Likes = _postLikeRepository.getLikes(userPost),
                IsLiked = _postLikeRepository.isLiked(User.Identity.Name)
            });
        }
    }
}