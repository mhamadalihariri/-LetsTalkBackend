using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Let_sTalk.Models;


namespace Let_sTalk.Data
{
    public interface IPreferenceRepository
    {
        Preference create(Preference pref);
        Preference getByName(string name);
        Preference getById(int id);
        List<Preference> getAll();
        void deleteById(Preference pref);
    }
}
