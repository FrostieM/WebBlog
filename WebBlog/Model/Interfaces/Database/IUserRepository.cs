using System.Linq;

namespace WebBlog.Model.Interfaces.Database
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void SaveUser(User user);
        void DeleteUser(User user);
    }
}