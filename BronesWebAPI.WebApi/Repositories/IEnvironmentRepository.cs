namespace BronesWebAPI.WebApi.Repositories
{
    public interface IEnvironmentRepository
    {
        Task<IEnumerable<Models.Environments>> GetAll();
        Task<IEnumerable<Models.Environments?>> GetById(Guid Id);
        Task<IEnumerable<Models.Environments?>> GetByUserId(string OwnerUserId);
        Task Add(Models.Environments environments);
        Task Update(Guid id, Models.Environments updatedEnvironment);
        Task Delete(string Id);
        Task<IEnumerable<Models.Environments>> ReadAsync();
        Task InsertAsync(Guid EnvironmentId, string Route, Guid OwnerUserId);
        Task UpdateAsync(string Id, Models.Environments updatedEnvironment);
        Task DeleteAsync(string Id);
    }
}
