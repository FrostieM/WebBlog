using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebBlog.Controllers;
using WebBlog.Model;
using WebBlog.Model.Interfaces.Repositories;
using WebBlog.Model.ViewData;
using WebBlogTests.FakeData;
using Xunit;

namespace WebBlogTests
{
    public class CommentLikeControllerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ICommentRepository> _commentRepository = new Mock<ICommentRepository>();
        private readonly Mock<ICommentLikeRepository> _commentLikeRepository = new Mock<ICommentLikeRepository>();
        
        
        public CommentLikeControllerTests()
        {
            var users = FakeRepositories.FakeUsers.ToList();
            var blogs = FakeRepositories.GetFakeBlogs(users).ToList();
            var posts = FakeRepositories.GetFakePosts(blogs).ToList();
            var comments = FakeRepositories.GetFakeComments(posts, users).ToList();
            var commentsLike = FakeRepositories.GetFakeCommentLikes(comments, users);
            
            _userRepository.Setup(r => r.Users).Returns(users.AsQueryable);
            
            _commentRepository.Setup(r => r.Comments).Returns(comments.AsQueryable);
            
            _commentLikeRepository.Setup(r => r.CommentLikes).Returns(commentsLike.AsQueryable);
        }

        [Fact]
        public void CannotLikeIt_UserNotFound()
        {
            var controller = new CommentLikeController(
                _userRepository.Object,
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("testFail", "User")
            };
            
            var result = controller.CommentLike(1) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("User not found", result.Value);
        }
        
        [Fact]
        public void CannotLikeIt_CommentNotFound()
        {
            var controller = new CommentLikeController(
                _userRepository.Object,
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.CommentLike(0) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Comment not found", result.Value);
        }
        
        [Fact]
        public void CanUnLikeComment()
        {
            var controller = new CommentLikeController(
                _userRepository.Object,
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.CommentLike(1) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            _commentLikeRepository.Verify(m => 
                m.DeleteCommentLike(It.IsAny<CommentLike>()), Times.Once);
            
            var comment = result.Value as ItemViewData<Comment>;
            Assert.NotNull(comment);
        }
        
        [Fact]
        public void CanLikeComment()
        {
            var controller = new CommentLikeController(
                _userRepository.Object,
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test2", "User")
            };
            
            var result = controller.CommentLike(3) as ObjectResult;
            
            _commentLikeRepository.Verify(m => 
                m.SaveCommentLike(It.IsAny<CommentLike>()), Times.Once);
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            
            var comment = result.Value as ItemViewData<Comment>;
            Assert.NotNull(comment);
        }
    }
}