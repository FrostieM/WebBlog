using System.Collections.Generic;

namespace WebBlog.Model.ViewData
{
    public class PostsInfo
    {
        public string Type { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public int CurrentPage { get; set; } 
    }
}