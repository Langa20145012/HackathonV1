using Hackathon_Application_UI.Models;

namespace Hackathon_Application_UI.Interface
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetDocumentsByMatterIdAsync(int matterId);
        Task<Document> GetDocumentByIdAsync(int documentId);
        Task<Document> UploadDocumentAsync(int matterId, IFormFile file, string status);
        Task<Document> UpdateDocumentStatusAsync(int documentId, string status);
        Task<bool> DeleteDocumentAsync(int documentId);
        Task<byte[]> DownloadDocumentAsync(int documentId);
    }
}
