namespace WebBlog.Model.ViewData
{
    public class LikeViewData<T> where T: class
    {
        public T Item { get; set; }
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
    }
}