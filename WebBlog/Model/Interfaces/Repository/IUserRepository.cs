using System.Linq;

namespace WebBlog.Model.Interfaces.Repository
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void SaveUser(User user);
        void DeleteUser(User user);
    }
}