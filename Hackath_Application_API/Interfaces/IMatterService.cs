using Hackathon_Application_Database.Models;

namespace Hackath_Application_API.Interfaces
{
    public interface IMatterService
    {
        Task<IEnumerable<Matter>> GetAllMattersAsync();
        Task<Matter> GetMatterByIdAsync(int matterId);
        Task<Matter> CreateMatterAsync(Matter matter);
        Task<Matter> UpdateMatterAsync(Matter matter);
        Task<bool> DeleteMatterAsync(int matterId);
    }
}
