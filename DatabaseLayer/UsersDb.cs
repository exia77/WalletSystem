using BusinessObjects;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace DatabaseLayer
{
    public class UsersDb
    {
        private readonly string _connectionString;
        public UsersDb(string connectionString) 
        { 
            _connectionString = connectionString;
        }

        public async Task<bool> Register(UsersModelObject users)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("Users_SetNewUsers", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", users.Username);
                command.Parameters.AddWithValue("@Password", HashPassword(users.Password));

                bool result = false;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = bool.Parse(reader["Result"].ToString()!);
                    }
                }
                await connection.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Register. {ex.Message}");
            }
        }

        public async Task<bool> CheckUserExist(UsersModelObject users)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("Users_CheckUserExist", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", users.Username);

                bool result = false;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = bool.Parse(reader["Result"].ToString()!);
                    }
                }
                await connection.CloseAsync();
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception($"Error in Verify. {ex.Message}");
            }
        }

        public async Task<bool> VerifyUser(UsersModelObject users)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("Users_GetVerifyUsers", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", users.Username);
                command.Parameters.AddWithValue("@Password", HashPassword(users.Password));

                bool result = false;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = bool.Parse(reader["Result"].ToString()!);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Verify. {ex.Message}");
            }

        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }
    }
}
