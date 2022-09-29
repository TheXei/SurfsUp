using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RentDto
    {
        public int BoardId { get; set; }
        public DateTime StartRent { get; set; }
        public DateTime EndRent { get; set; }
        public string? UserName { get; set; }
    }
}
