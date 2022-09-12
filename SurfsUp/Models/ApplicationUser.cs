using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SurfsUp.Models
{
    /* It creates a class that inherits from IdentityUser. */
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string  State { get; set; }
        public string PostalCode { get; set; }
    }
}
