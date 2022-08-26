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
        public string Name { get; set; }
        //public string Description { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Thickness { get; set; }
        public float Volume { get; set; }
        public BoardType Type { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string Equipments { get; set; }
        public string ImageURL { get; set; }

        //public ICollection<Equipment> Equipments { get; set; }
    }
}
