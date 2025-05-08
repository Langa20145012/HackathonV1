using Hackathon_Application_Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackath_Application_API.Interfaces
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetDocumentsByMatterIdAsync(int matterId);
        Task<Document> GetDocumentByIdAsync(int documentId);
        Task<Document> CreateDocumentAsync(Document document);
        Task<Document> UpdateDocumentAsync(Document document);
        Task<bool> DeleteDocumentAsync(int documentId);
        Task<Document> UpdateDocumentStatusAsync(int documentId, string status);
    }
}
