using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Let_sTalk.Models
{
    public class Preference
    {
        [Key]
        public int Id { get; set; }
        public string CuisineName { get; set; }
        public string CuisineCountry { get; set; }
        public virtual List<UserPreference>? UserPreferences { get; set; }
        //public int IsDeleted { get; set; }
    }
}
