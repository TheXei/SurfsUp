using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfsUp.Models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string? IPAddress { get; set; }

        public ICollection<Rent>? Rents { get; set; }
    }
}
