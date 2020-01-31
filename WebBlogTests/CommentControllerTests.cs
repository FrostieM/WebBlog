using System.Collections.Generic;
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
    public class CommentControllerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<IPostRepository> _postRepository = new Mock<IPostRepository>();
        private readonly Mock<ICommentRepository> _commentRepository = new Mock<ICommentRepository>();
        private readonly Mock<ICommentLikeRepository> _commentLikeRepository = new Mock<ICommentLikeRepository>();

        public CommentControllerTests()
        {
            var users = FakeRepositories.FakeUsers.ToList();
            var blogs = FakeRepositories.GetFakeBlogs(users).ToList();
            var posts = FakeRepositories.GetFakePosts(blogs).ToList();
            var comments = FakeRepositories.GetFakeComments(posts, users).ToList();
            var commentLikes = FakeRepositories.GetFakeCommentLikes(comments, users);
            
            _userRepository.Setup(u => u.Users).Returns(users.AsQueryable());

            _postRepository.Setup(p => p.Posts).Returns(posts.AsQueryable());
            
            _commentRepository.Setup(c => c.Comments).Returns(comments.AsQueryable());

            _commentLikeRepository.Setup(c => c.CommentLikes).Returns(commentLikes.AsQueryable());
        }

        [Fact]
        public void CannotGetComments_UserNotFound()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("testFail", "User")
            };
            
            var result = controller.GetComments(1) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("User not found", result.Value);
        }
        
        [Fact]
        public void CannotGetComments_PostNotFound()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.GetComments(0) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Post not found", result.Value);
        }

        [Fact]
        public void CanGetComments_WithoutParentComments()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.GetComments(1) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            var comments = result.Value as IEnumerable<ItemViewData<Comment>>;
            
            Assert.NotNull(comments);
            Assert.Equal(3, comments.Count());
        }
        
        [Fact]
        public void CanGetComments_WithParentComment()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.GetComments(1, 1) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            var comments = result.Value as IEnumerable<ItemViewData<Comment>>;
            
            Assert.NotNull(comments);
            Assert.Equal(2, comments.Count());
        }
        
        [Fact]
        public void CannotSaveComment_UserNotFound()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("testFail", "User")
            };
            
            var result = controller.SaveComment(1, "testContent") as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("User not found", result.Value);
        }
        
        [Fact]
        public void CannotSaveComment_PostNotFound()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.SaveComment(0, "testContent") as ObjectResult;
            
            _commentRepository.Verify(c => c.SaveComment(It.IsAny<Comment>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Post not found", result.Value);
        }
        
        [Fact]
        public void CannotSaveComment_ParentCommentNotFound()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.SaveComment(1, "testContent", 0) as ObjectResult;
            
            _commentRepository.Verify(c => c.SaveComment(It.IsAny<Comment>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Parent comment not found", result.Value);
        }
        
        [Fact]
        public void CanSaveComment_withoutParentComment()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.SaveComment(1, "testContent") as ObjectResult;
            
            _commentRepository.Verify(c => c.SaveComment(It.IsAny<Comment>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            var comment = result.Value as ItemViewData<Comment>;
            Assert.IsType<ItemViewData<Comment>>(comment);
            Assert.Equal("test1", comment.Item.User.UserName);
            Assert.Equal(1, comment.Item.Post.Id);
            Assert.Equal("testContent", comment.Item.Content);
            Assert.Null(comment.Item.ParentComment);
        }
        
        [Fact]
        public void CanSaveComment_withParentComment()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.SaveComment(1, "testContent", 1) as ObjectResult;
            
            _commentRepository.Verify(c => c.SaveComment(It.IsAny<Comment>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            var comment = result.Value as ItemViewData<Comment>;
            Assert.IsType<ItemViewData<Comment>>(comment);
            Assert.Equal("test1", comment.Item.User.UserName);
            Assert.Equal(1, comment.Item.Post.Id);
            Assert.Equal("testContent", comment.Item.Content);
            Assert.NotNull(comment.Item.ParentComment);
        }
        
        [Fact]
        public void CannotDeleteComment_UserNotFound()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("testFail", "User")
            };
            
            var result = controller.DeleteComment(1) as ObjectResult;
            
            _commentRepository.Verify(c => c.DeleteComment(It.IsAny<Comment>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("User not found", result.Value);
        }
        
        [Fact]
        public void CannotDeleteComment_CommentNotFound()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test2", "User")
            };
            
            var result = controller.DeleteComment(1) as ObjectResult;
            
            _commentRepository.Verify(c => c.DeleteComment(It.IsAny<Comment>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Comment not found", result.Value);
        }
        
        [Fact]
        public void CanDeleteComment()
        {
            var controller = new CommentController(
                _userRepository.Object, 
                _postRepository.Object, 
                _commentRepository.Object,
                _commentLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.DeleteComment(1) as ObjectResult;
            
            _commentRepository.Verify(c => c.DeleteComment(It.IsAny<Comment>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Deleted", result.Value);
        }
    }
}