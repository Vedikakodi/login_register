using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DataStore.Implementation.Repositories {
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection is missing in appsettings.json");
        }

        public async Task<User> GetUserByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Email = @Email",
                    new { Email = email }
                );
            }
        }


        public async Task AddUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (Name, Email, PasswordHash) VALUES (@Name, @Email, @PasswordHash)";
                var parameters = new { user.Name, user.Email, user.PasswordHash };

                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
