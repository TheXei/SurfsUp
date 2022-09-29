using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfsUp.Models
{
    public class Rent
    {
        [Key]
        public string RentId { get; set; }
        [ForeignKey("Board")]
        public int BoardId { get; set; }
        [Display(Name = "Start of rent")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartRent { get; set; } = DateTime.Now;
        [Display(Name = "End of rent")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndRent { get; set; } = DateTime.Now.AddDays(7);
        public virtual Board? Board { get; set; }
        
        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public Rent()
        {
            RentId = Guid.NewGuid().ToString();
        }
    }
}
