using System.Linq;

namespace WebBlog.Model.Interfaces.Repositories
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void SaveUser(User user);
        void DeleteUser(User user);
    }
}