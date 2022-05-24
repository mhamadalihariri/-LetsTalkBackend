using Let_sTalk.Models;
using System.Collections.Generic;
using System.Json;

namespace LetsTalkBackend.DTOS
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string? Image { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        //public Preference Preference { get; set; }
        public List<Preference> Preferences { get; set; }
        public Location Location { get; set; }
        public List<Match> Matches { get; set; }
        public string FirebaseId { get; set; }
    }
}
