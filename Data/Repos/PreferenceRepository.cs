using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Let_sTalk.Models;
using Let_sTalk.Data.Context;


namespace Let_sTalk.Data.Repos
{
    public class PreferenceRepository : IPreferenceRepository
    {
        private readonly MyDbContext _dbContext;
        public PreferenceRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        Preference IPreferenceRepository.create(Preference pref)
        {
            _dbContext.preferences.Add(pref);
            pref.Id = _dbContext.SaveChanges();
            return pref;
        }

        void IPreferenceRepository.deleteById(Preference pref)
        {
            _dbContext.preferences.Remove(pref);
            _dbContext.SaveChanges();
        }

        List<Preference> IPreferenceRepository.getAll()
        {
            return _dbContext.preferences.ToList();
        }

        Preference IPreferenceRepository.getById(int id)
        {
            return _dbContext.preferences.Find(id);
        }

        Preference IPreferenceRepository.getByName(string name)
        {
            return _dbContext.preferences.FirstOrDefault(p => p.CuisineName == name);
        }
    }
}
