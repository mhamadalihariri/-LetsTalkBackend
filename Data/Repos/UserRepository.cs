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

        public void ChangePassword(int id, string password)
        {
            var existingUser = _dbContext.users.FirstOrDefault(x => x.Id == id);
            if (existingUser != null)
            {
                existingUser.Password = password;
                _dbContext.users.Update(existingUser);
                _dbContext.SaveChanges();
            }
        }

        public User createOrUpdate(User user)
        {
            var existingUser = _dbContext.users.FirstOrDefault(u => u.Email == user.Email);
            if(existingUser == null)
            {
            _dbContext.users.Add(user);

            }else
            {
                _dbContext.users.Update(user);
            }
            _dbContext.SaveChanges();
            return user;
        }

        public User getByEmail(string email)
        {
            Console.WriteLine("Email " + email);
            User user = _dbContext.users.FirstOrDefault(u => u.Email == email);
            if(user != null)
            {
                //Console.WriteLine(user.Email);
                List<UserPreference> foundUserPreference = _dbContext.userPreferences.Where(up => up.UserId == user.Id).ToList();
                user.UserPreferences = foundUserPreference;
            }
            return user;
        }

        public User getById(int id)
        {
            User user = _dbContext.users.Find(id);
            List<UserPreference> foundUserPreference = _dbContext.userPreferences.Where(up => up.UserId == id).ToList();
            user.UserPreferences = foundUserPreference;
            return user;
        }

    }
}
