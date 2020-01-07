using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Model.Interfaces.Repositories;
using WebBlog.Model.Repositories;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostTagsController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogRepository _blogRepository;

        public PostTagsController(ITagRepository tagRepository, IBlogRepository blogRepository)
        {
            _tagRepository = tagRepository;
            _blogRepository = blogRepository;
        }

        [Authorize]
        [HttpGet, Route("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string username, string type)
        {
            var blog = _blogRepository.Blogs.FirstOrDefault(b => b.User.UserName == username);
            if (blog == null) return NotFound("User not found");

            return Ok(_tagRepository.GetBlogTags(blog, type));
        }
    }
}