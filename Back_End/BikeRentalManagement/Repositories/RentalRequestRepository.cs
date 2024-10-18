using BikeRentalManagement.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;


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
            try
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
            }catch(Exception ex)
            {
                return null;
            }


        }

        public async Task<int> ApproveRentalRequest(int requestId, DateTime approvalDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "UPDATE RentalRequests SET Status = 'approved', ApprovalDate = @ApprovalDate WHERE Id = @Id",
                    connection);
                command.Parameters.AddWithValue("@ApprovalDate", approvalDate);
                command.Parameters.AddWithValue("@Id", requestId);
                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<RentalRequest> GetRentalRequestById(int id)
        {
            RentalRequest rentalRequest = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM RentalRequests WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    rentalRequest = new RentalRequest
                    {
                        RentalRequestId = (int)reader["Id"],
                        MotorbikeId = (int)reader["MotorbikeId"],
                        UserId = (int)reader["UserId"],
                        RequestDate = (DateTime)reader["RequestDate"],
                        Status = reader["Status"].ToString()
                    };
                }
            }
            return rentalRequest;
        }
    }

}
