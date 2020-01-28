using System.Collections.Generic;

namespace WebBlog.Model.ViewData
{
    public class UserPostsViewData
    {
        public IEnumerable<LikeViewData<Post>> Posts { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}