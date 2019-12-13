using System.Linq;
using WebBlog.Model.Interfaces.Repository;

namespace WebBlog.Model.EfData
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public EFUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IQueryable<User> Users => _context.Users;
        
        public void SaveUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}