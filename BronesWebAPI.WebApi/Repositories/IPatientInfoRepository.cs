using BronesWebAPI.WebApi.Models;

namespace BronesWebAPI.WebApi.Repositories
{
    public interface IPatientInfoRepository
    {
        Task Add(PatientInfo users);
        Task Delete(Guid UserId);
        Task DeleteAsync(Guid UserId);
        Task<IEnumerable<PatientInfo>> GetAll();
        Task<IEnumerable<PatientInfo?>> GetById(Guid UserId);
        Task InsertAsync(Guid UserId, Guid OwnerUserId, string Name, DateOnly DateOfBirth, string Behandelplan, string NaamArts, DateOnly EersteAfspraak);
        Task<IEnumerable<PatientInfo>> ReadAsync();
        Task Update(Guid UserId, PatientInfo updatedUser);
        Task UpdateAsync(Guid UserId, PatientInfo updatedUsers);
    }
}