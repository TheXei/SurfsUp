namespace SurfsUp.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public DateTime StartRent { get; set; } = DateTime.Now;
        public DateTime EndRent { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }
    }
}
