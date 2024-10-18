using BikeRentalManagement.Models;
using BikeRentalManagement.Repositories;

namespace BikeRentalManagement.Services
{

    public class MotorbikeService
    {
        private readonly MotorbikeRepository _motorbikeRepository;

        public MotorbikeService(MotorbikeRepository motorbikeRepository)
        {
            _motorbikeRepository = motorbikeRepository;
        }

        public bool AddMotorbike(Motorbike motorbike)
        {
            return _motorbikeRepository.AddMotorbike(motorbike);
        }

        public Motorbike GetMotorbikeById(int motorbikeId)
        {
            return _motorbikeRepository.GetMotorbikeById(motorbikeId);
        }

        public bool UpdateMotorbike(Motorbike motorbike)
        {
            return _motorbikeRepository.UpdateMotorbike(motorbike);
        }

        public bool DeleteMotorbike(int motorbikeId)
        {
            return _motorbikeRepository.DeleteMotorbike(motorbikeId);
        }

        public List<Motorbike> GetAllMotorbikes()
        {
            return _motorbikeRepository.GetAllMotorbikes();
        }
    }

}
