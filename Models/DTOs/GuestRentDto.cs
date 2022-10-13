using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class GuestRentDto
    {
        public int BoardId { get; set; }
        public DateTime StartRent { get; set; }
        public DateTime EndRent { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IPAddress { get; set; }
    }
}
