using BikeRentalManagement.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


namespace BikeRentalManagement.Repositories
{
   
    public class RentalRequestRepository
    {
        private readonly string _connectionString;

        public RentalRequestRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddRentalRequest(RentalRequest request)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO RentalRequests (MotorbikeId, UserId, RequestDate, Status) VALUES (@MotorbikeId, @UserId, @RequestDate, @Status)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MotorbikeId", request.MotorbikeId);
                command.Parameters.AddWithValue("@UserId", request.UserId);
                command.Parameters.AddWithValue("@RequestDate", request.RequestDate);
                command.Parameters.AddWithValue("@Status", request.Status);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public RentalRequest GetRentalRequestById(int rentalRequestId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM RentalRequests WHERE RentalRequestId = @RentalRequestId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RentalRequestId", rentalRequestId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new RentalRequest
                    {
                        RentalRequestId = (int)reader["RentalRequestId"],
                        MotorbikeId = (int)reader["MotorbikeId"],
                        UserId = (int)reader["UserId"],
                        RequestDate = (DateTime)reader["RequestDate"],
                        Status = reader["Status"].ToString(),
                        ApprovalDate = (DateTime)(reader["ApprovalDate"] == DBNull.Value ? null : (DateTime?)reader["ApprovalDate"])
                    };
                }

                return null;
            }
        }

        public bool UpdateRentalRequestStatus(int rentalRequestId, string status, DateTime? approvalDate = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE RentalRequests SET Status = @Status, ApprovalDate = @ApprovalDate WHERE RentalRequestId = @RentalRequestId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@ApprovalDate", approvalDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@RentalRequestId", rentalRequestId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0;
            }
        }

        public List<RentalRequest> GetAllRentalRequests()
        {
            List<RentalRequest> requests = new List<RentalRequest>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM RentalRequests";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    requests.Add(new RentalRequest
                    {
                        RentalRequestId = (int)reader["RentalRequestId"],
                        MotorbikeId = (int)reader["MotorbikeId"],
                        UserId = (int)reader["UserId"],
                        RequestDate = (DateTime)reader["RequestDate"],
                        Status = reader["Status"].ToString(),
                        ApprovalDate = (DateTime)(reader["ApprovalDate"] == DBNull.Value ? null : (DateTime?)reader["ApprovalDate"])
                    });
                }
            }

            return requests;
        }
    }

}
