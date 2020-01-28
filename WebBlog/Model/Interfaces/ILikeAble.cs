namespace WebBlog.Model.Interfaces
{
    public interface ILikeAble
    {
        int GetLikes(int id);
        bool IsLiked(string username, int id);
    }
}