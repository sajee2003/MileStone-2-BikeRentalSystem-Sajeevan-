using BikeRentalManagement.Models;
using BikeRentalManagement.Repositories;

namespace BikeRentalManagement.Services
{
    // File: Services/RentalService.cs
    public class RentalService
    {
        private readonly RentalRepository _rentalRepository;

        public RentalService(RentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public bool AddRental(Rental rental)
        {
            return _rentalRepository.AddRental(rental);
        }

        public Rental GetRentalById(int rentalId)
        {
            return _rentalRepository.GetRentalById(rentalId);
        }

        public bool DeleteRental(int rentalId)
        {
            return _rentalRepository.DeleteRental(rentalId);
        }

        public List<Rental> GetAllRentals()
        {
            return _rentalRepository.GetAllRentals();
        }
        public async Task<List<Rental>> GetOverdueRentals()
        {
            return await _rentalRepository.GetOverdueRentals();
        }
    }

}
