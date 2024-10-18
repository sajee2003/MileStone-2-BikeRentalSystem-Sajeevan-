using BikeRentalManagement.Models;
using BikeRentalManagement.Repositories;

namespace BikeRentalManagement.Services
{
    // File: Services/RentalRequestService.cs
    public class RentalRequestService
    {
        private readonly RentalRequestRepository _requestRepository;
        private readonly RentalRepository _rentalRepository;
        public RentalRequestService(RentalRequestRepository rentalRequestRepository, RentalRepository rentalRepository)
        {
            _requestRepository = rentalRequestRepository;
            _rentalRepository = rentalRepository;
        }


        public bool AddRentalRequest(RentalRequest request)
        {
            return _requestRepository.AddRentalRequest(request);
        }

        
        public async Task<bool> ApproveRentalRequest(int requestId)
        {
            var rentalRequest = await _requestRepository.GetRentalRequestById(requestId);
            if (rentalRequest == null || rentalRequest.Status != "pending") return false;

  
            await _requestRepository.ApproveRentalRequest(requestId, DateTime.UtcNow);
            var rental = new Rental
            {
                MotorbikeId = rentalRequest.MotorbikeId,
                UserId = rentalRequest.UserId,
                RentDate = DateTime.UtcNow
            };
             _rentalRepository.AddRental(rental);
            return true;
        }

        public bool UpdateRentalRequestStatus(int requestId, string status, DateTime? approvalDate = null)
        {
            return _requestRepository.UpdateRentalRequestStatus(requestId, status, approvalDate);
        }

        public List<RentalRequest> GetAllRentalRequests()
        {
            return _requestRepository.GetAllRentalRequests();
        }

        public async Task<RentalRequest> GetRentalRequestById(int id)
        {
            return await _requestRepository.GetRentalRequestById(id);
        }

    }

}
