using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Let_sTalk.Models
{
    public class Match
    {
        [Key]
        public int IdMatch { get; set; }
        public  int User1 { get; set; }
        public  int User2 { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
        public int IsDeleted { get; set; }
        public int IsMatched { get; set; }
    }
}
