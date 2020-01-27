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
    public class PostLikeTests
    {
        private readonly Mock<IPostRepository> _postRepository;
        private readonly Mock<IPostLikeRepository> _postLikeRepository;
        private readonly Mock<IUserRepository> _userRepository;
        
        public PostLikeTests()
        {
            var users = FakeRepositories.FakeUsers.ToList();
            var blogs = FakeRepositories.GetFakeBlogs(users).ToList();
            var posts = FakeRepositories.GetFakePosts(blogs).ToList();
            var postsLikes = FakeRepositories.GetFakePostLikes(posts, users);
            
            var usersMock = new Mock<IUserRepository>();
            usersMock.Setup(c => c.Users).Returns(users.AsQueryable);
            _userRepository = usersMock;
            
            var postsMock = new Mock<IPostRepository>();
            postsMock.Setup(c => c.Posts).Returns(posts.AsQueryable);
            _postRepository = postsMock;
            
            var postLikeMock = new Mock<IPostLikeRepository>();
            postLikeMock.Setup(l => l.PostLikes).Returns(postsLikes.AsQueryable);
            _postLikeRepository = postLikeMock;
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
                m.DeletePostLikes(It.IsAny<PostLike>()), Times.Once);
            
            var post = result.Value as PostViewData;
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
                m.SavePostLikes(It.IsAny<PostLike>()), Times.Once);
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            
            var post = result.Value as PostViewData;
            Assert.NotNull(post);
        }
    }
}