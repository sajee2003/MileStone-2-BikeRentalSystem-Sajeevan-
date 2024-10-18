using BikeRentalManagement.Models;
using Microsoft.Data.SqlClient;

namespace BikeRentalManagement.Repositories
{
    public class OrderHistoryRepository
    {
        private readonly string _connectionString;

        public OrderHistoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddOrderHistory(OrderHistory order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO OrderHistory (MotorbikeId, UserId, RentDate, ReturnDate) VALUES (@MotorbikeId, @UserId, @RentDate, @ReturnDate)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MotorbikeId", order.MotorbikeId);
                command.Parameters.AddWithValue("@UserId", order.UserId);
                command.Parameters.AddWithValue("@RentDate", order.RentDate);
                command.Parameters.AddWithValue("@ReturnDate", order.ReturnDate);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public List<OrderHistory> GetAllOrderHistories()
        {
            List<OrderHistory> histories = new List<OrderHistory>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM OrderHistory";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    histories.Add(new OrderHistory
                    {
                        OrderId = (int)reader["OrderId"],
                        MotorbikeId = (int)reader["MotorbikeId"],
                        UserId = (int)reader["UserId"],
                        RentDate = (DateTime)reader["RentDate"],
                        ReturnDate = (DateTime)reader["ReturnDate"]
                    });
                }
            }

            return histories;
        }
    }
}
