using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Let_sTalk.Models
{
    public class Reservation
    {
        [Key]
        public int IdReservation { get; set; }
        public string ApiUrl { get; set; }
        public string Date { get; set; }

    }
}
