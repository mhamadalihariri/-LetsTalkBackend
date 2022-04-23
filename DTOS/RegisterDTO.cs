using Let_sTalk.Models;

namespace Let_sTalk.DTOS
{
    public class RegisterDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Dob { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public Preference Preference { get; set; }
    }
}
