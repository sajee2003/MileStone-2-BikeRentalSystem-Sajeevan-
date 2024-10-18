using BikeRentalManagement.Models;
using Microsoft.Data.SqlClient;

namespace BikeRentalManagement.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User GetUser(string username, string password, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password AND Role = @Role";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Role", role);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        UserId = (int)reader["UserId"],
                        UserName = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        NIC = reader["NIC"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }

                return null;
            }
        }

        public bool RegisterUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (Username, Password, NIC, Role) VALUES (@Username, @Password, @NIC, @Role)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.UserName);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@NIC", user.NIC);
                command.Parameters.AddWithValue("@Role", user.Role);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public User GetUserById(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        UserId = (int)reader["UserId"],
                        UserName = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        NIC = reader["NIC"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }

                return null;
            }
        }

        public bool UpdateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Users SET Username = @Username, Password = @Password, NIC = @NIC, Role = @Role WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.UserName);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@NIC", user.NIC);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@UserId", user.UserId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public bool DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Users WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserId = (int)reader["UserId"],
                        UserName = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        NIC = reader["NIC"].ToString(),
                        Role = reader["Role"].ToString()
                    });
                }
            }

            return users;
        }
    }
}
