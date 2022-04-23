using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Let_sTalk.Models
{
    public class Location
    {
        [Key]
        public int IdLocation { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }

    }
}
