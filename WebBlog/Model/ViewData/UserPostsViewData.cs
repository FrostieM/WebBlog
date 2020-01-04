using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebBlog.Model.ViewData
{
    public class UserPostsViewData
    {
        public IEnumerable<PostViewData> Posts { get; set; }
        public bool isCreator { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}