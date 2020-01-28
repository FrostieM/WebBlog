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
    public class CommentLikeController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentLikeRepository _commentLikeRepository;

        public CommentLikeController(
            IUserRepository userRepository,
            ICommentRepository commentRepository,
            ICommentLikeRepository commentLikeRepository)
        {
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _commentLikeRepository = commentLikeRepository;
        }


        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CommentLike(int commentId)
        {
            var currentUser = _userRepository.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (currentUser == null) return NotFound("User not found");

            var comment = _commentRepository.Comments.FirstOrDefault(c => c.Id == commentId);
            
            if (comment == null) return NotFound("Comment not found");

            var commentLike = _commentLikeRepository.CommentLikes
                .FirstOrDefault(c => c.User.Equals(currentUser) && c.Comment.Equals(comment));
            
            if (commentLike == null) //like is not exist
            {
                _commentLikeRepository.SaveCommentLike(new CommentLike 
                {
                    Comment = comment, 
                    User = currentUser
                });
            }
            else //unlike comment
            {
                _commentLikeRepository.DeleteCommentLike(commentLike);
            }

            return Ok(new LikeViewData<Comment>
            {
                Likes = _commentLikeRepository.GetLikes(commentId),
                IsLiked = _commentLikeRepository.IsLiked(User.Identity.Name, commentId)
            });
        }
    }
}