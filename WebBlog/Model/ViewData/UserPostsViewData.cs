using System;

namespace WebBlog.Model.ViewData
{
    public class UserPostsViewData
    {
        public Array Posts { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}