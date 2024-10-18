using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalManagement.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        [ForeignKey("Motorbikes")]
        public int MotorbikeId { get; set; }
        [ForeignKey("Users")]
        public int UserId{ get; set; }
        public DateTime RentDate { get; set; }
    }
}
