using System;

namespace WebBlog.Model.Forms
{
    public class PostForm
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public Array tags { get; set; }
    }
}