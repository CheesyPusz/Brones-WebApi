using Microsoft.Data.SqlClient;
using Dapper;
using BronesWebAPI.WebApi.Models;

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


        //public async Task<PatientInfo?> GetById(Guid Id)
        //{
        //    using (var sqlConnection = new SqlConnection(_sqlConnectionString))
        //    {
        //        var query = "SELECT * FROM [PatientInfo] WHERE OwnerUserId = @Id";
        //        return await sqlConnection.QuerySingleOrDefaultAsync<PatientInfo>(query, new { Id });
        //    }
        //}

        public async Task<PatientInfo?> GetById(Guid OwnerUserId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "SELECT * FROM [PatientInfo] WHERE OwnerUserId = @OwnerUserId";
                var parameters = new { OwnerUserId };

                var result = await sqlConnection.QuerySingleOrDefaultAsync<PatientInfo>(query, parameters);
                return result;
            }
        }

        public async Task Add(Models.PatientInfo users)
        {
            if (!users.UserId.HasValue || users.UserId == Guid.Empty)
            {
                users.UserId = Guid.NewGuid();
            }
            await InsertAsync(users.UserId.Value, users.OwnerUserId.Value, users.Name, users.DateOfBirth, users.BehandelPlan, users.NaamArts, users.EersteAfspraak, users.PositionX);
        }

        //public async Task Update(Guid UserId, Models.PatientInfo updatedUser)
        //{
        //    var user = await GetById(UserId);
        //    //if (user != null)
        //    //{
        //    //    await UpdateAsync(UserId, updatedUser);
        //    //}
        //}

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

        public async Task InsertAsync(Guid UserId, Guid OwnerUserId, string Name, string DateOfBirth, string Behandelplan, string NaamArts, string EersteAfspraak, float PositionX)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "INSERT INTO [PatientInfo] (UserId, OwnerUserId, Name, DateOfBirth, Behandelplan, NaamArts, EersteAfspraak, PositionX) VALUES (@UserId, @OwnerUserId, @Name, @DateOfBirth, @Behandelplan, @NaamArts, @EersteAfspraak, @PositionX)";

                await sqlConnection.ExecuteAsync(query, new
                {
                    UserId,
                    OwnerUserId,
                    Name,
                    DateOfBirth,
                    Behandelplan,
                    NaamArts,
                    EersteAfspraak,
                    PositionX
                });

                //try
                //{
                //    var result = await sqlConnection.ExecuteAsync(query, new
                //    {
                //        UserId,
                //        OwnerUserId,
                //        Name,
                //        DateOfBirth = TimeOnly.MinValue,
                //        Behandelplan,
                //        NaamArts,
                //        EersteAfspraak = TimeOnly.MinValue
                //    });
                //    Console.WriteLine("Rows affected: " + result);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Error executing query: " + ex.Message);
                //}
            }
        }


        public async Task UpdateAsync(Guid OwnerUserId, float PositionX)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = @"UPDATE [PatientInfo] 
                              SET PositionX = @PositionX
                              WHERE OwnerUserId = @OwnerUserId";

                await sqlConnection.ExecuteAsync(query, new { PositionX, OwnerUserId });
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
