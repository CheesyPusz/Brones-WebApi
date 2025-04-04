using BronesWebAPI.WebApi.Models;

namespace BronesWebAPI.WebApi.Repositories
{
    public interface IPatientInfoRepository
    {
        Task Add(PatientInfo users);
        Task Delete(Guid UserId);
        Task DeleteAsync(Guid UserId);
        Task<IEnumerable<PatientInfo>> GetAll();
        Task<PatientInfo?> GetById(Guid Id);
        Task InsertAsync(Guid UserId, Guid OwnerUserId, string Name, string DateOfBirth, string Behandelplan, string NaamArts, string EersteAfspraak, float PositionX);
        Task<IEnumerable<PatientInfo>> ReadAsync();
        //Task Update(Guid UserId, PatientInfo updatedUser);
        Task UpdateAsync(Guid OwnerUserId, float PositionX);
    }
}