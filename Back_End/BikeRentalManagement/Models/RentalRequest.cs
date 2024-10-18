using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalManagement.Models
{
    public class RentalRequest
    {
        public int RentalRequestId { get; set; }
        [ForeignKey("Motorbikes")]
        public int MotorbikeId { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public DateTime ApprovalDate { get; set; }

    }
}
