
using System.Text;
using System.Text.Json;
using Hackath_Application_API.Interfaces;
using Hackathon_Application_Database.DatabaseContext;
using Hackathon_Application_Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;


namespace Hackath_Application_API.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly HttpClient _httpClient;
        private readonly string _notificationServiceUrl;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public DocumentService(
            ApplicationDbContext dbContext,
            IDistributedCache cache,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _cache = cache;
            _httpClient = httpClient;
            _notificationServiceUrl = configuration["ServiceUrls:NotificationService"];
        }

        public async Task<IEnumerable<Document>> GetDocumentsByMatterIdAsync(int matterId)
        {
            string cacheKey = $"Documents_Matter_{matterId}";
            string cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<List<Document>>(cachedData);
            }

            var documents = await _dbContext.Documents
                .Where(d => d.MatterId == matterId)
                .ToListAsync();

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(documents),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiration }
            );

            return documents;
        }

        public async Task<Document> GetDocumentByIdAsync(int documentId)
        {
            string cacheKey = $"Document_{documentId}";
            string cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<Document>(cachedData);
            }

            var document = await _dbContext.Documents.FindAsync(documentId);

            if (document != null)
            {
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(document),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiration }
                );
            }

            return document;
        }

        public async Task<Document> CreateDocumentAsync(Document document)
        {
            document.CreatedDate = DateTime.UtcNow;
            document.LastModifiedDate = DateTime.UtcNow;

            _dbContext.Documents.Add(document);
            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync($"Documents_Matter_{document.MatterId}");

            return document;
        }

        public async Task<Document> UpdateDocumentAsync(Document document)
        {
            var existingDocument = await _dbContext.Documents.FindAsync(document.DocumentId);

            if (existingDocument == null)
                return null;

            existingDocument.FileName = document.FileName;
            existingDocument.ContentType = document.ContentType;
            existingDocument.FileContent = document.FileContent;
            existingDocument.LastModifiedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync($"Documents_Matter_{document.MatterId}");
            await _cache.RemoveAsync($"Document_{document.DocumentId}");

            return existingDocument;
        }

        public async Task<bool> DeleteDocumentAsync(int documentId)
        {
            var document = await _dbContext.Documents.FindAsync(documentId);

            if (document == null)
                return false;

            _dbContext.Documents.Remove(document);
            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync($"Documents_Matter_{document.MatterId}");
            await _cache.RemoveAsync($"Document_{documentId}");

            return true;
        }

        public async Task<Document> UpdateDocumentStatusAsync(int documentId, string status)
        {
            var document = await _dbContext.Documents.FindAsync(documentId);

            if (document == null)
                return null;

            string oldStatus = document.Status;
            document.Status = status;
            document.LastModifiedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync($"Documents_Matter_{document.MatterId}");
            await _cache.RemoveAsync($"Document_{documentId}");

            // Notify if status changed
            if (oldStatus != status)
            {
                await NotifyStatusChangeAsync(document);
            }

            return document;
        }

        private async Task NotifyStatusChangeAsync(Document document)
        {
            var notification = new
            {
                DocumentId = document.DocumentId,
                MatterId = document.MatterId,
                Status = document.Status,
                FileName = document.FileName
            };

            var content = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json");

            await _httpClient.PostAsync($"{_notificationServiceUrl}/api/notification/document-status-changed", content);
        }
    }
}
