namespace BikeRentalManagement.Models
{
    public class UpdateStatusRequest
    {
        public string RequestId { get; set; }
        public string Status {  get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
