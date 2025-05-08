using Hackathon_Application_UI.Models;

namespace Hackathon_Application_UI.Interface
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