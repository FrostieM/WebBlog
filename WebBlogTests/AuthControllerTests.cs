using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebBlog.Controllers;
using WebBlog.Model;
using WebBlog.Model.Forms;
using WebBlog.Model.Interfaces.Repositories;
using WebBlogTests.FakeData;
using Xunit;

namespace WebBlogTests
{
    public class AuthControllerTests
    {

        private readonly Mock<IUserRepository> _repository = new Mock<IUserRepository>();
        
        public AuthControllerTests()
        {
            var users = FakeRepositories.FakeUsers;
            _repository.Setup(c => c.Users).Returns(users.AsQueryable);
        }

        [Fact]
        public void CannotLogIn()
        {
            var controller = new AuthController(_repository.Object);
            var result = controller.SignIn(new LoginForm
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
            var result = controller.SignIn(new LoginForm
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
            var result = controller.SignUp(new SignUpForm
            {
                UserName = "test1",
                Password = "testPassword1",
                Firstname = "testName",
                Lastname = "testLastName",
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
            var result = controller.SignUp(new SignUpForm
            {
                UserName = "testOK",
                Password = "testPasswordOK",
                Firstname = "testName1",
                Lastname = "testLastName2",
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