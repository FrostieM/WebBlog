using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebBlog.Controllers;
using WebBlog.Model;
using WebBlog.Model.Forms;
using WebBlog.Model.Interfaces.Repositories;
using WebBlogTests.Helpers;
using Xunit;

namespace WebBlogTests
{
    public class AuthControllerTests
    {

        private readonly Mock<IUserRepository> _repository;
        
        public AuthControllerTests()
        {
            var users = FakeData.FakeUsers;
            
            var mock = new Mock<IUserRepository>();
            mock.Setup(c => c.Users).Returns(users.AsQueryable);

            _repository = mock;
        }

        [Fact]
        public void CannotLogIn()
        {
            var controller = new AuthController(_repository.Object);
            var result = controller.Post(new LoginForm
            {
                UserName = "test0",
                Password = "testPassword0"
            }) as ObjectResult;
            
            _repository.Verify(m => m.SaveUser(It.IsAny<User>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("User not found", result.Value);
        }

        [Fact]
        public void CanLogIn()
        {
            var controller = new AuthController(_repository.Object);
            var result = controller.Post(new LoginForm
            {
                UserName = "test1",
                Password = "testPassword1"
            }) as ObjectResult;
            
            _repository.Verify(m => m.SaveUser(It.IsAny<User>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public void CannotSignUp()
        {
            var controller = new AuthController(_repository.Object);
            var result = controller.Post(new SignUpForm
            {
                UserName = "test1",
                Password = "testPassword1",
                RePassword = "testPassword1",
                FirstName = "testName",
                LastName = "testLastName",
                Email = "test email"
            }) as ObjectResult;
            
            _repository.Verify(m => m.SaveUser(It.IsAny<User>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("User already exist", result.Value);
        }

        [Fact]
        public void CanSignUp()
        {
            var controller = new AuthController(_repository.Object);
            var result = controller.Post(new SignUpForm
            {
                UserName = "testOK",
                Password = "testPasswordOK",
                RePassword = "testPasswordOK",
                FirstName = "testName1",
                LastName = "testLastName2",
                Email = "test emailOK"
            }) as ObjectResult;
            
            _repository.Verify(m => m.SaveUser(It.IsAny<User>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
        }
    }
}