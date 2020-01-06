using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WebBlog.Model.Forms
{
    public class PostForm
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public IFormFile File { get; set; }
    }
}