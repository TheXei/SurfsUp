using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SurfsUp.Models
{
    public enum BoardType {
        [Display(Name = "Shortboard")]
        Shortboard,
        [Display(Name = "Funboard")]
        Funboard,
        [Display(Name = "Fish")]
        Fish,
        [Display(Name = "Longboard")]
        Longboard,
        [Display(Name = "SUP")]
        SUP
    }
    public class Board
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        //public string Description { get; set; }

        [Range(1, 100)]
        [Required]
        public float Length { get; set; }

        [Range(1, 100)]
        [Required]
        public float Width { get; set; }

        [Range(1, 100)]
        [Required]
        public float Thickness { get; set; }

        [Range(1, 1000)]
        [Required]
        public float Volume { get; set; }

        [Required]
        public BoardType Type { get; set; }

        [Range(1, 10000)]
        [Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal Price { get; set; }


        public string Equipments { get; set; }

        [Required]
        public string ImageURL { get; set; }

        //public ICollection<Equipment> Equipments { get; set; }
    }
}
