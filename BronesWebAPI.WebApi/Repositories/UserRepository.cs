using Microsoft.Data.SqlClient;
using Dapper;

namespace BronesWebAPI.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private string _sqlConnectionString;

        public UserRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        public async Task<IEnumerable<Models.Users>> GetAll()
        {
            return await ReadAsync();
        }


        public async Task<IEnumerable<Models.Users?>> GetById(Guid UserId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Models.Users>("SELECT * FROM [PatientInfo] WHERE UserId = @UserId", new { UserId = UserId });
            }
        }

        public async Task Add(Models.Users users)
        {
            await InsertAsync(users.UserId, users.Name, users.BirthDate, users.BehandelPlan, users.NaamArts, users.EersteAfspraak);
        }

        public async Task Update(Guid UserId, Models.Users updatedUser)
        {
            var user = await GetById(UserId);
            if (user != null)
            {
                await UpdateAsync(UserId, updatedUser);
            }
        }

        public async Task Delete(Guid UserId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "DELETE FROM [PatientInfo] WHERE UserId = @UserId";
                await sqlConnection.ExecuteAsync(query, new { UserId = UserId });
            }
        }

        public async Task<IEnumerable<Models.Users>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Models.Users>("SELECT * FROM [PatientInfo]");
            }
        }

        public async Task InsertAsync(Guid UserId, string Name, DateOnly DateOfBirth, string Behandelplan, string NaamArts, DateOnly EersteAfspraak)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "INSERT INTO [PatientInfo](UserId, Name, DateOfBirth, Behandelplan, NaamArts, EersteAfspraak) VALUES (@UserID, @Name, @DateOfBirth, @Behandelplan, @NaamArts, @EersteAfspraak)";

                await sqlConnection.ExecuteAsync(query, new { UserId, Name, DateOfBirth, Behandelplan, NaamArts, EersteAfspraak });
            }
        }

        public async Task UpdateAsync(Guid UserId, Models.Users updatedUsers)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = @"UPDATE [PatientInfo] 
                              SET UserId = @NewUserId, Name = @Name, BirthDate = @BirthDate, BehandelPlan = @BehandelPlan, NaamArts = @NaamArts, EersteAfspraak = @EersteAfspraak
                              WHERE UserId = @OldUserId";

                await sqlConnection.ExecuteAsync(query, new { NewUserId = updatedUsers.UserId, updatedUsers.Name, updatedUsers.BirthDate, updatedUsers.BehandelPlan, updatedUsers.NaamArts, updatedUsers.EersteAfspraak, OldUserId = UserId });
            }
        }

        public async Task DeleteAsync(Guid UserId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "DELETE FROM [PatientInfo] WHERE UserId = @UserId";
                await sqlConnection.ExecuteAsync(query, new { UserId = UserId });
            }
        }
    }
}
