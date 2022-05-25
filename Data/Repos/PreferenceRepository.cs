using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Let_sTalk.Models;
using Let_sTalk.Data.Context;
using LetsTalkBackend.DTOS;
using LetsTalkBackend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Let_sTalk.Data.Repos
{
    public class PreferenceRepository : IPreferenceRepository
    {
        private readonly MyDbContext _dbContext;
        private readonly GeoLocationService locationService;
        public PreferenceRepository(MyDbContext dbContext, GeoLocationService geoLocationService)
        {
            _dbContext = dbContext;
            locationService = geoLocationService;
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

        public HashSet<UserDTO> getUsersHavingSamePreference(string userEmail, double rangeInKm)
        {
            User mainUser = _dbContext.users.Where(u => u.Email == userEmail).Include(u=> u.Location).FirstOrDefault();
            Console.WriteLine("main userrrr mfff " + mainUser.Email);
            List<int> PreferencesIds = _dbContext.userPreferences.Where(p => p.UserId == mainUser.Id).Select(p=> p.PreferenceId).ToList();
            Console.WriteLine("prefss " + PreferencesIds.Count);
            HashSet<UserDTO> users = new HashSet<UserDTO>();
            foreach(int PreferenceId in PreferencesIds)
            {
            //Console.WriteLine(PreferenceId);
            //Console.WriteLine("-----------");
            //Console.WriteLine(mainUser.Id);
                List<UserPreference> userPreferences = _dbContext.userPreferences.Where(up => up.PreferenceId == PreferenceId && up.UserId != mainUser.Id).ToList();
                List<Match> allMatches = _dbContext.matches.ToList();
                foreach (Match match in allMatches)
                {
                    foreach (UserPreference up in userPreferences)
                    {
                        User foundUser = _dbContext.users.Include(x => x.Location).FirstOrDefault( u=> u.Id == up.UserId);
                        if (foundUser != null && (match.User1 != up.UserId && match.User2 != up.UserId))
                        {
                            //Console.WriteLine("main user Longitude : "+ mainUser.Location.Longitude);
                            //Console.WriteLine("main user Latitude :" + mainUser.Location.Latitude);

                            //Console.WriteLine("foundUser user Longitude : " + foundUser.Location.Longitude);
                            //Console.WriteLine("foundUser user Latitude :" + foundUser.Location.Latitude);

                            //Console.WriteLine("range in km :"+ rangeInKm);
                            //Console.WriteLine("before distance");
                            var distance = locationService.GetDistance(mainUser.Location.Longitude, mainUser.Location.Latitude, foundUser.Location.Longitude, foundUser.Location.Latitude);
                            //Console.WriteLine("after distance");

                            //Console.WriteLine("distancess " + distance); 
                                
                                //Here we are comparing the distance between the main actor, and all the users having the same preference if they are within a specific area
                            if (distance < rangeInKm * 1000)
                            {
                                Console.WriteLine("User is in the area");
                                UserDTO dto = new UserDTO();
                                List<Preference> preferences = new List<Preference>();
                                List<UserPreference> userPrefs = foundUser.UserPreferences;
                                foreach (UserPreference pref in userPrefs)
                                {
                                    preferences.Add(getById(pref.PreferenceId));
                                }
                                dto.Id = up.UserId;
                                dto.PhoneNumber = foundUser.PhoneNumber;
                                dto.Firstname = foundUser.FirstName;
                                dto.Lastname = foundUser.LastName;
                                dto.Email = foundUser.Email;
                                dto.Preferences = preferences;
                                dto.Gender = foundUser.Gender;
                                dto.FirebaseId = foundUser.FirebaseId;
                                dto.Location = foundUser.Location;
                                dto.Image = foundUser.Image;
                                dto.DOB = foundUser.DOB;
                                dto.PhoneNumber = foundUser.PhoneNumber;

                                bool userExists = users.Any(u => u.Email == dto.Email);
                                if(!userExists)
                                users.Add(dto);

                            } 
                        }
                    }
                }

            }
            //Console.WriteLine("users to return count : " + users.Count);
            users.ToList().ForEach(us =>Console.WriteLine(us.Firstname));
            return users;
        }

        public HashSet<Preference> getPreferencesByUserId(int id)
        {
            List<int> prefsIds = _dbContext.userPreferences.Where(up => up.UserId == id).Select(up => up.PreferenceId).ToList();
            HashSet<Preference> preferences = new HashSet<Preference>();
            foreach (int prefId in prefsIds)
            {
                Preference pref = _dbContext.preferences.Find(prefId);
                preferences.Add(pref);
            }
            return preferences;
        }
    }
}
