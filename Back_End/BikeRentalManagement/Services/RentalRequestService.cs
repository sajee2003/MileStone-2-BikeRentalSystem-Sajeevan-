using BikeRentalManagement.Models;
using BikeRentalManagement.Repositories;

namespace BikeRentalManagement.Services
{
    // File: Services/RentalRequestService.cs
    public class RentalRequestService
    {
        private readonly RentalRequestRepository _requestRepository;

        public RentalRequestService(RentalRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public bool AddRentalRequest(RentalRequest request)
        {
            return _requestRepository.AddRentalRequest(request);
        }

        public RentalRequest GetRentalRequestById(int requestId)
        {
            return _requestRepository.GetRentalRequestById(requestId);
        }

        public bool UpdateRentalRequestStatus(int requestId, string status, DateTime? approvalDate = null)
        {
            return _requestRepository.UpdateRentalRequestStatus(requestId, status, approvalDate);
        }

        public List<RentalRequest> GetAllRentalRequests()
        {
            return _requestRepository.GetAllRentalRequests();
        }
    }

}
