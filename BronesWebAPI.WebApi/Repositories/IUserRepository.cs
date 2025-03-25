using BronesWebAPI.WebApi.Models;

namespace BronesWebAPI.WebApi.Repositories
{
    public interface IUserRepository
    {
        Task Add(Users users);
        Task Delete(Guid UserId);
        Task DeleteAsync(Guid UserId);
        Task<IEnumerable<Users>> GetAll();
        Task<IEnumerable<Users?>> GetById(Guid UserId);
        Task InsertAsync(Guid UserId, string Name, DateOnly DateOfBirth, string Behandelplan, string NaamArts, DateOnly EersteAfspraak);
        Task<IEnumerable<Users>> ReadAsync();
        Task Update(Guid UserId, Users updatedUser);
        Task UpdateAsync(Guid UserId, Users updatedUsers);
    }
}