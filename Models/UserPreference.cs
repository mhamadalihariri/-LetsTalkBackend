namespace Let_sTalk.Models
{
    public class UserPreference
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PreferenceId { get; set; }
        public User User { get; set; }
        public Preference Preference { get; set; }
        public int IsDeleted { get; set; }
    }
}
