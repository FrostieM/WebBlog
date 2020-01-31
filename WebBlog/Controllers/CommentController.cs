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
    public class CommentController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentLikeRepository _commentLikeRepository;

        public CommentController(
            IUserRepository userRepository, 
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            ICommentLikeRepository commentLikeRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _commentLikeRepository = commentLikeRepository;
        }

        [Authorize]
        [HttpGet, Route("getComments/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetComments(int postId, int? parentId = null)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null) return NotFound("User not found");

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == postId);
            if (post == null) return NotFound("Post not found");

            Comment parentComment = null;
            if (parentId != null)
            {
                parentComment = _commentRepository.Comments.FirstOrDefault(c => c.Id == parentId);
                if (parentComment == null) return NotFound("Parent comment not found");
            }
            
            return Ok(_commentRepository.Comments
                .Where(c => c.Post.Equals(post) && c.ParentComment == parentComment)
                .Select(c => new ItemViewData<Comment>
                {
                    Item = c,
                    Likes = c.Likes.Count,
                    IsLiked = _commentLikeRepository.IsLiked(User.Identity.Name, c.Id),
                    Comments = c.SubComments.Count
                }));
        }

        [Authorize]
        [HttpPost, Route("saveComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SaveComment([FromBody] CommentForm commentForm)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null) return NotFound("User not found");

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == commentForm.PostId);
            if (post == null) return NotFound("Post not found");

            Comment parentComment = null;
            if (commentForm.CommentId != null)
            {
                parentComment = _commentRepository.Comments.FirstOrDefault(c => c.Id == commentForm.CommentId);
                if (parentComment == null) return NotFound("Parent comment not found");
            }

            var comment = new Comment()
            {
                Post = post,
                User = user,
                Content = commentForm.Content,
                ParentComment = parentComment
            };
            
            _commentRepository.SaveComment(comment);

            return Ok(new ItemViewData<Comment>
            {
                Item = comment,
                Comments = 0,
                Likes = 0,
                IsLiked = false
            });
        }

        [Authorize]
        [HttpGet, Route("deleteComment")]
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