using Microsoft.Data.SqlClient;
using Dapper;

namespace BronesWebAPI.WebApi.Repositories
{
    public class PatientInfoRepository : IPatientInfoRepository
    {
        private string _sqlConnectionString;

        public PatientInfoRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        public async Task<IEnumerable<Models.PatientInfo>> GetAll()
        {
            return await ReadAsync();
        }


        public async Task<IEnumerable<Models.PatientInfo?>> GetById(Guid OwnerUserId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Models.PatientInfo>("SELECT * FROM [PatientInfo] WHERE OwnerUserId = @OwnerUserId", new { OwnerUserId = OwnerUserId });
            }
        }

        public async Task Add(Models.PatientInfo users)
        {
            if (!users.UserId.HasValue || users.UserId == Guid.Empty)
            {
                users.UserId = Guid.NewGuid();
            }
            await InsertAsync(users.UserId.Value, users.OwnerUserId.Value, users.Name, DateOnly.FromDateTime(users.BirthDate), users.BehandelPlan, users.NaamArts, DateOnly.FromDateTime(users.EersteAfspraak));
        }

        public async Task Update(Guid UserId, Models.PatientInfo updatedUser)
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

        public async Task<IEnumerable<Models.PatientInfo>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Models.PatientInfo>("SELECT * FROM [PatientInfo]");
            }
        }

        public async Task InsertAsync(Guid UserId, Guid OwnerUserId, string Name, DateOnly DateOfBirth, string Behandelplan, string NaamArts, DateOnly EersteAfspraak)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "INSERT INTO [PatientInfo] (UserId, OwnerUserId, Name, DateOfBirth, Behandelplan, NaamArts, EersteAfspraak) VALUES (@UserId, @OwnerUserId, @Name, @DateOfBirth, @Behandelplan, @NaamArts, @EersteAfspraak)";

                try
                {
                    var result = await sqlConnection.ExecuteAsync(query, new
                    {
                        UserId,
                        OwnerUserId,
                        Name,
                        DateOfBirth = DateOfBirth.ToDateTime(TimeOnly.MinValue),
                        Behandelplan,
                        NaamArts,
                        EersteAfspraak = EersteAfspraak.ToDateTime(TimeOnly.MinValue)
                    });
                    Console.WriteLine("Rows affected: " + result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error executing query: " + ex.Message);
                }
            }
        }


        public async Task UpdateAsync(Guid UserId, Models.PatientInfo updatedUsers)
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
