using BikeRentalManagement.Models;
using Microsoft.Data.SqlClient;

namespace BikeRentalManagement.Repositories
{
    public class RentalRepository
    {
        private readonly string _connectionString;

        public RentalRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddRental(Rental rental)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Rentals (MotorbikeId, UserId, RentDate) VALUES (@MotorbikeId, @UserId, @RentDate)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MotorbikeId", rental.MotorbikeId);
                command.Parameters.AddWithValue("@UserId", rental.UserId);
                command.Parameters.AddWithValue("@RentDate", rental.RentDate);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public Rental GetRentalById(int rentalId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Rentals WHERE RentalId = @RentalId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RentalId", rentalId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Rental
                    {
                        RentalId = (int)reader["RentalId"],
                        MotorbikeId = (int)reader["MotorbikeId"],
                        UserId = (int)reader["UserId"],
                        RentDate = (DateTime)reader["RentDate"]
                    };
                }

                return null;
            }
        }

        public bool DeleteRental(int rentalId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Rentals WHERE RentalId = @RentalId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RentalId", rentalId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public List<Rental> GetAllRentals()
        {
            List<Rental> rentals = new List<Rental>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Rentals";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rentals.Add(new Rental
                    {
                        RentalId = (int)reader["RentalId"],
                        MotorbikeId = (int)reader["MotorbikeId"],
                        UserId = (int)reader["UserId"],
                        RentDate = (DateTime)reader["RentDate"]
                    });
                }
            }

            return rentals;
        }

        public async Task<List<Rental>> GetOverdueRentals()
        {
            var overdueRentals = new List<Rental>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "SELECT * FROM Rentals WHERE ReturnDate IS NULL AND DATEDIFF(MINUTE, RentDate, GETDATE()) > 1",
                    connection);
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    overdueRentals.Add(new Rental
                    {
                        RentalId = (int)reader["Id"],
                        MotorbikeId = (int)reader["MotorbikeId"],
                        UserId = (int)reader["UserId"],
                        RentDate = (DateTime)reader["RentDate"]
                    });
                }
            }
            return overdueRentals;
        }
    }
}
