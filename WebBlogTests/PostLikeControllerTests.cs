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
    public class PostLikeControllerTests
    {
        private readonly Mock<IPostRepository> _postRepository = new Mock<IPostRepository>();
        private readonly Mock<IPostLikeRepository> _postLikeRepository = new Mock<IPostLikeRepository>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        
        public PostLikeControllerTests()
        {
            var users = FakeRepositories.FakeUsers.ToList();
            var blogs = FakeRepositories.GetFakeBlogs(users).ToList();
            var posts = FakeRepositories.GetFakePosts(blogs).ToList();
            var postsLikes = FakeRepositories.GetFakePostLikes(posts, users);
            
            _userRepository.Setup(r => r.Users).Returns(users.AsQueryable);
            
            _postRepository.Setup(r => r.Posts).Returns(posts.AsQueryable);
            
            _postLikeRepository.Setup(r => r.PostLikes).Returns(postsLikes.AsQueryable);
        }

        [Fact]
        public void CannotLikeIt_UserNotFound()
        {
            var controller = new PostLikeController(
                _userRepository.Object,
                _postRepository.Object,
                _postLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test0", "User")
            };
            
            var result = controller.PostLike(1) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("User not found", result.Value);
        }
        
        [Fact]
        public void CannotLikeIt_PostNotFound()
        {
            var controller = new PostLikeController(
                _userRepository.Object,
                _postRepository.Object,
                _postLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.PostLike(0) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Post not found", result.Value);
        }
        
        [Fact]
        public void CanUnLikePost()
        {
            var controller = new PostLikeController(
                _userRepository.Object,
                _postRepository.Object,
                _postLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.PostLike(1) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            _postLikeRepository.Verify(m => 
                m.DeletePostLike(It.IsAny<PostLike>()), Times.Once);
            
            var post = result.Value as ItemViewData<Post>;
            Assert.NotNull(post);
        }
        
        [Fact]
        public void CanLikePost()
        {
            var controller = new PostLikeController(
                _userRepository.Object,
                _postRepository.Object,
                _postLikeRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test2", "User")
            };
            
            var result = controller.PostLike(3) as ObjectResult;
            
            _postLikeRepository.Verify(m => 
                m.SavePostLike(It.IsAny<PostLike>()), Times.Once);
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            
            var post = result.Value as ItemViewData<Post>;
            Assert.NotNull(post);
        }
    }
}