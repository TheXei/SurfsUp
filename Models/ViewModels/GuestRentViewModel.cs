using SurfsUp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class GuestRentViewModel
    {
        [Required]
        public Rent Rent { get; set; }
        [Required]
        public Guest Guest { get; set; }
    }
}
