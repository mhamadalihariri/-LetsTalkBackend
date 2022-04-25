using Let_sTalk.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Let_sTalk.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public string? Image { get; set; }
        public string Gender { get; set; }
        public virtual Location? Location { get; set; }

        [NotMapped]
        public List<Match> Matches { get; set; }
        public int IsDeleted { get; set; }
        public virtual List<UserPreference>? UserPreferences { get; set; }
        public string FirebaseId { get; set; }

        public User()
        {
            
        }
    }
}
