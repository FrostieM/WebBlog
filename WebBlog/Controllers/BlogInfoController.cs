using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Model.Interfaces.Repositories;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogInfoController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogRepository _blogRepository;

        public BlogInfoController(ITagRepository tagRepository, IBlogRepository blogRepository)
        {
            _tagRepository = tagRepository;
            _blogRepository = blogRepository;
        }
        
        [Authorize]
        [HttpGet, Route("getTags/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTags(string username, string type)
        {
            var blog = _blogRepository.Blogs.FirstOrDefault(b => b.User.UserName == username);
            if (blog == null) return NotFound("User not found");

            return Ok(_tagRepository.GetBlogTags(blog, type));
        }
        
        [Authorize]
        [HttpGet, Route("getUserInfo/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInfo(string username)
        {
            var user = _blogRepository.Blogs.Where(b => b.User.UserName == username)
                .Select(b => b.User).FirstOrDefault();
            if (user == null) return NotFound("User not found");

            return Ok(user);
        }
    }
}