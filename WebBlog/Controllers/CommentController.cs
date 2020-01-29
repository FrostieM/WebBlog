using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Model;
using WebBlog.Model.Interfaces.Repositories;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentController(
            IUserRepository userRepository, 
            IPostRepository postRepository,
            ICommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        [Authorize]
        [HttpPost, Route("getComments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetComments(int postId)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null) return NotFound("User not found");

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == postId);
            if (post == null) return NotFound("Post not found");

            return Ok(_commentRepository.Comments.Where(c => c.Post.Equals(post) && c.ParentComment == null));
        }

        [Authorize]
        [HttpPost, Route("saveComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SaveComment(int postId, string content, int? commentId = null)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null) return NotFound("User not found");

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == postId);
            if (post == null) return NotFound("Post not found");

            Comment parentComment = null;
            if (commentId != null)
            {
                parentComment = _commentRepository.Comments.FirstOrDefault(c => c.Id == commentId);
                if (parentComment == null) return NotFound("Parent comment not found");
            }

            var comment = new Comment()
            {
                Post = post,
                User = user,
                Content = content,
                ParentComment = parentComment
            };
            
            _commentRepository.SaveComment(comment);

            return Ok(comment);
        }

        [Authorize]
        [HttpPost, Route("deleteComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteComment(int commentId)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null) return NotFound("User not found");

            var comment = _commentRepository.Comments
                .FirstOrDefault(c => c.Id == commentId && c.User.Equals(user));
            if (comment == null) return NotFound("Comment not found");

            
            _commentRepository.DeleteComment(comment);
            
            return Ok("Deleted");
        }
    }
}