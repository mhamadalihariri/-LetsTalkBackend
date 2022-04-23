using Let_sTalk.Data.Context;
using Let_sTalk.Data.IRepos;
using Let_sTalk.Models;
using System.Collections.Generic;
using System.Linq;

namespace Let_sTalk.Data.Repos
{
    public class UserPreferenceRepository : IUserPreferenceReporsitory
    {
        private readonly MyDbContext _dbContext;

        public UserPreferenceRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public UserPreference create(UserPreference userPreference)
        {
            _dbContext.userPreferences.Add(userPreference);
            userPreference.Id = _dbContext.SaveChanges();
            return userPreference;
        }

        public void delete(UserPreference userPreference)
        {
            _dbContext.userPreferences.Remove(userPreference);
            _dbContext.SaveChanges();
        }

        public List<UserPreference> GetAllUserPreferences()
        {
            return _dbContext.userPreferences.ToList();
        }

        public UserPreference GetUserPreferenceById(int id)
        {
            return _dbContext.userPreferences.Find(id);
        }

        public List<UserPreference> GetUserPreferencesByPreferenceId(int prefId)
        {
            return _dbContext.userPreferences.Where(up => up.PreferenceId == prefId).ToList();

        }

        public List<UserPreference> GetUserPreferencesByUserId(int userId)
        {
            return _dbContext.userPreferences.Where(up => up.UserId == userId).ToList();
        }

        public UserPreference update(UserPreference userPreference)
        {
            UserPreference existingUserPreference = GetUserPreferenceById(userPreference.Id);
            if (existingUserPreference != null)
            {
                _dbContext.userPreferences.Update(userPreference);
                userPreference.Id = _dbContext.SaveChanges();
                return existingUserPreference;
            }else return null;

        }
    }
}
