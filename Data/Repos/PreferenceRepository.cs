using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Let_sTalk.Models;
using Let_sTalk.Data.Context;
using LetsTalkBackend.DTOS;

namespace Let_sTalk.Data.Repos
{
    public class PreferenceRepository : IPreferenceRepository
    {
        private readonly MyDbContext _dbContext;
        public PreferenceRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Preference create(Preference pref)
        {
            _dbContext.preferences.Add(pref);
            _dbContext.SaveChanges();
            return pref;
        }

        public void deleteById(Preference pref)
        {
            _dbContext.preferences.Remove(pref);
            _dbContext.SaveChanges();
        }

        public List<Preference> getAll()
        {
            return _dbContext.preferences.ToList();
        }

        public Preference getById(int id)
        {
            return _dbContext.preferences.Find(id);
        }

        public Preference getByName(string name)
        {
            return _dbContext.preferences.FirstOrDefault(p => p.CuisineName == name);
        }

        public HashSet<UserDTO> getUsersByPreferenceId(int preferenceId)
        {
            List<int> idUsersByPreference = _dbContext.userPreferences.Where(up => up.PreferenceId == preferenceId).Select(up=> up.UserId).ToList();
           HashSet<UserDTO> users = new HashSet<UserDTO>();
            foreach (int idUser in idUsersByPreference)
            {
                if(_dbContext.users.Find(idUser) != null)
                {
                    UserDTO dto = new UserDTO();
                    User foundUser = _dbContext.users.Find(idUser);
                    dto.Id = idUser;
                    dto.PhoneNumber = foundUser.PhoneNumber;
                    dto.Firstname = foundUser.FirstName;
                    dto.Lastname = foundUser.LastName;
                    dto.Email = foundUser.Email;
                    dto.UserPreferences = foundUser.UserPreferences;
                    dto.Gender = foundUser.Gender;
                    dto.FirebaseId = foundUser.FirebaseId;
                    dto.Location = foundUser.Location;
                    dto.Image = foundUser.Image;
                    dto.PhoneNumber = foundUser.PhoneNumber;
                    users.Add(dto);
                }
            }

            return users;
        }
    }
}
