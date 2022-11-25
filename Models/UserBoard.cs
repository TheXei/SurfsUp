using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfsUp.Models
{
    public class UserBoard : Board
    {
        [Required]
        public string OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }
    }
}
