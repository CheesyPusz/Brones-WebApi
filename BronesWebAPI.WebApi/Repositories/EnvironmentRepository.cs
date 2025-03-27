using Microsoft.Data.SqlClient;
using Dapper;
using BronesWebAPI.WebApi.Models;
using System.Diagnostics;

namespace BronesWebAPI.WebApi.Repositories

{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private string _sqlConnectionString;
        public EnvironmentRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        public async Task<IEnumerable<Models.Environments>> GetAll()
        {
            return await ReadAsync();
        }

        public async Task<IEnumerable<Models.Environments?>> GetById(Guid Id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "SELECT * FROM [Environments] WHERE EnvironmentId = @Id";
                return await sqlConnection.QueryAsync<Models.Environments>(query, new { Id = Id });
            }
        }

        public async Task<IEnumerable<Models.Environments?>> GetByUserId(string OwnerUserId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "SELECT * FROM [Environments] WHERE OwnerUserId = @Id";
                return await sqlConnection.QueryAsync<Models.Environments>(query, new { Id = OwnerUserId });
            }
        }

        public async Task Add(Models.Environments environments)
        {
            Console.WriteLine("Reached Add method");
            //_environments.Add(environment);
            await InsertAsync(environments.EnvironmentId, environments.Route, environments.OwnerUserId);
            Console.WriteLine("Executed InsertAsync method");
        }

        public async Task Update(Guid id, Models.Environments updatedEnvironment)
        {
            var environment = await GetById(id);
            if (environment != null)
            {
                await UpdateAsync(id.ToString(), updatedEnvironment);
            }
        }

        public async Task Delete(string Id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "DELETE FROM [Environments] WHERE EnvironmentId = @Id";
                await sqlConnection.ExecuteAsync(query, new { Id = Id });
            }
        }

        public async Task<IEnumerable<Models.Environments>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Models.Environments>("SELECT * FROM [Environments]");
            }
        }

        public async Task InsertAsync(Guid EnvironmentId, string Route, Guid OwnerUserId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "INSERT INTO [Environments] (EnvironmentId, Route, OwnerUserId) VALUES (@EnvironmentId, @Route, @OwnerUserId)";

                await sqlConnection.ExecuteAsync(query, new { EnvironmentId, Route, OwnerUserId });
            }
        }

        public async Task UpdateAsync(string Id, Models.Environments updatedEnvironment)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = @"UPDATE [Environments] 
                              SET Route = @Route
                              WHERE EnvironmentId = @OldEnvironmentId";

                await sqlConnection.ExecuteAsync(query, new { NewEnvironmentId = updatedEnvironment.EnvironmentId, updatedEnvironment.Route, updatedEnvironment.OwnerUserId, OldEnvironmentId = Id });
            }
        }

        public async Task DeleteAsync(string EnvironmentId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "DELETE FROM [Environments] WHERE EnvironmentId = @Id";
                await sqlConnection.ExecuteAsync(query, new { Id = EnvironmentId });
            }
        }

        //public async Task<Environment> CreateEnvironment(string name)
        //{
        //    var sql = "INSERT INTO Environment2D (Name, MaxHeight, MaxLength) VALUES (@Name, @MaxHeight, @MaxLength)";
        //    return await sqlConnection.QuerySingleOrDefaultAsync<Environment>(sql, new { Name = name, MaxHeight = 15, MaxLength = 15 });
        //}

        //public async Task<Environment?> ReadAsync(int id)
        //{
        //    using (var sqlConnection = new SqlConnection(_sqlConnectionString))
        //    {
        //        return await sqlConnection.QuerySingleOrDefaultAsync<Environment>("SELECT * FROM [Environment2D] WHERE Id = @Id", new { Id = id });
        //    }
        //}
    }
}