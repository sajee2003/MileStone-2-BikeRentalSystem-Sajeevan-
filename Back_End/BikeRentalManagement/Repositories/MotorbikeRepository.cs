using BikeRentalManagement.Models;
using Microsoft.Data.SqlClient;

namespace BikeRentalManagement.Repositories
{
    public class MotorbikeRepository
    {
        private readonly string _connectionString;

        public MotorbikeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddMotorbike(Motorbike motorbike)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Motorbikes (RegNumber, Brand, Model, Category, ImageData) VALUES (@RegNumber, @Brand, @Model, @Category, @ImageData)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RegNumber", motorbike.RegNumber);
                command.Parameters.AddWithValue("@Brand", motorbike.Brand);
                command.Parameters.AddWithValue("@Model", motorbike.Model);
                command.Parameters.AddWithValue("@Category", motorbike.Category);
                command.Parameters.AddWithValue("@ImageData", motorbike.ImageData ?? (object)DBNull.Value);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public Motorbike GetMotorbikeById(int motorbikeId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Motorbikes WHERE MotorbikeId = @MotorbikeId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MotorbikeId", motorbikeId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Motorbike
                    {
                        MotorbikeId = (int)reader["MotorbikeId"],
                        RegNumber = reader["RegNumber"].ToString(),
                        Brand = reader["Brand"].ToString(),
                        Model = reader["Model"].ToString(),
                        Category = reader["Category"].ToString(),
                        ImageData = reader["ImageData"].ToString()
                    };
                }

                return null;
            }
        }

        public bool UpdateMotorbike(Motorbike motorbike)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Motorbikes SET RegNumber = @RegNumber, Brand = @Brand, Model = @Model, Category = @Category, ImageData = @ImageData WHERE MotorbikeId = @MotorbikeId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RegNumber", motorbike.RegNumber);
                command.Parameters.AddWithValue("@Brand", motorbike.Brand);
                command.Parameters.AddWithValue("@Model", motorbike.Model);
                command.Parameters.AddWithValue("@Category", motorbike.Category);
                command.Parameters.AddWithValue("@ImageData", motorbike.ImageData ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@MotorbikeId", motorbike.MotorbikeId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public bool DeleteMotorbike(int motorbikeId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Motorbikes WHERE MotorbikeId = @MotorbikeId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MotorbikeId", motorbikeId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public List<Motorbike> GetAllMotorbikes()
        {
            List<Motorbike> motorbikes = new List<Motorbike>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Motorbikes";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    motorbikes.Add(new Motorbike
                    {
                        MotorbikeId = (int)reader["MotorbikeId"],
                        RegNumber = reader["RegNumber"].ToString(),
                        Brand = reader["Brand"].ToString(),
                        Model = reader["Model"].ToString(),
                        Category = reader["Category"].ToString(),
                        ImageData = reader["ImageData"].ToString()
                    });
                }
            }

            return motorbikes;
        }
    }
}
