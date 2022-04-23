using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Let_sTalk.Models;
using Let_sTalk.Data.Context;


namespace Let_sTalk.Data.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _dbContext;
        public UserRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public User create(User user)
        {
            _dbContext.users.Add(user);
            user.Id = _dbContext.SaveChanges();
            return user;
        }

        public User getByEmail(string email)
        {
            return _dbContext.users.FirstOrDefault(u => u.Email == email);
        }

        public User getById(int id)
        {
            return _dbContext.users.Find(id);
        }

        public User update(User user)
        {
            _dbContext.users.Update(user);
            user.Id = _dbContext.SaveChanges();
            return user;
        }
    }
}
