using BikeRentalManagement.Models;
using BikeRentalManagement.Repositories;

namespace BikeRentalManagement.Services
{
    
    public class OrderHistoryService
    {
        private readonly OrderHistoryRepository _orderRepository;

        public OrderHistoryService(OrderHistoryRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public bool AddOrderHistory(OrderHistory order)
        {
            return _orderRepository.AddOrderHistory(order);
        }

        public List<OrderHistory> GetAllOrderHistories()
        {
            return _orderRepository.GetAllOrderHistories();
        }
    }

}
