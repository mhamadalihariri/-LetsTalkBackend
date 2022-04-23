using Let_sTalk.Models;
using System.Collections.Generic;

namespace Let_sTalk.Data.IRepos
{
    public interface IUserPreferenceReporsitory
    {
        UserPreference create(UserPreference userPreference);
        UserPreference update(UserPreference userPreference);
        void delete(UserPreference userPreference);
        UserPreference GetUserPreferenceById(int id);
        List<UserPreference> GetUserPreferencesByUserId(int userId);
        List<UserPreference> GetUserPreferencesByPreferenceId(int prefId);

        List<UserPreference> GetAllUserPreferences();
    }
}
