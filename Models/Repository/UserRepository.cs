using crickinfo_mvc_ef_core.Models.Interfaces;

namespace crickinfo_mvc_ef_core.Models.Repository
{
    public class UserRepository : IUserRepo
    {
        private readonly CrickDbContext _context;

        public UserRepository(CrickDbContext crickDbContext)
        {
            _context = crickDbContext;
        }
        
        public User GetUserByEmailId(string emailId)
        {
            User user = _context.Users
                .Where(u => u.Email == emailId).FirstOrDefault();
            return user;
        }

        public User Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public string Delete(int id)
        {
            User u = _context.Users.Find(id);
            if(u != null)
            {
                _context.Users.Remove(u);
                _context.SaveChanges();
            }
            return "user deleted successfully";
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        public User Update(User user)
        {
            var u = _context.Users.Attach(user);
            u.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return user;
        }
    }
}
