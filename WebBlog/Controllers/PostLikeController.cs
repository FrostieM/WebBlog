using System.Linq;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PostLike(int postId)
        {
            var currentUser = _userRepository.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (currentUser == null) return NotFound("User not found");

            var userPost = _postRepository.Posts.FirstOrDefault(p => p.Id == postId);
            
            if (userPost == null) return NotFound("Post not found");

            var postLike = _postLikeRepository.PostLikes
                .FirstOrDefault(l => l.User.Equals(currentUser) && l.Post.Equals(userPost));
            
            if (postLike == null) //like is not exist
            {
                _postLikeRepository.SavePostLike(new PostLike 
                {
                    Post = userPost, 
                    User = currentUser
                });
            }
            else //unlike post
            {
                _postLikeRepository.DeletePostLike(postLike);
            }

            return Ok(new ItemViewData<Post>
            {
                Likes = _postLikeRepository.GetLikes(postId),
                IsLiked = _postLikeRepository.IsLiked(User.Identity.Name, postId)
            });
        }
    }
}