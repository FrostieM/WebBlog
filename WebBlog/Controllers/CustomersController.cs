using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[]
            {
                "First", User.Identity.Name
            };
        }
    }
}