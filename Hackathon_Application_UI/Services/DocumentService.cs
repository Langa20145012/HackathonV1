
using System;

using System.Collections.Generic;

using System.IO;

using System.Net.Http;

using System.Text;

using System.Text.Json;

using System.Threading.Tasks;
using global::Hackathon_Application_UI.Interface;
using global::Hackathon_Application_UI.Models;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Configuration;

namespace Hackathon_Application_UI.Services

{

    public class DocumentService : IDocumentService

    {

        private readonly HttpClient _httpClient;

        private readonly string _apiBaseUrl;

        public DocumentService(HttpClient httpClient, IConfiguration configuration)

        {

            _httpClient = httpClient;

            _apiBaseUrl = configuration["ApiSettings:GatewayUrl"];

        }

        public async Task<IEnumerable<Document>> GetDocumentsByMatterIdAsync(int matterId)

        {

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/document/matter/{matterId}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<Document>>(content, new JsonSerializerOptions

            {

                PropertyNameCaseInsensitive = true

            });

        }

        public async Task<Document> GetDocumentByIdAsync(int documentId)

        {

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/document/{documentId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)

                return null;

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Document>(content, new JsonSerializerOptions

            {

                PropertyNameCaseInsensitive = true

            });

        }

        public async Task<Document> UploadDocumentAsync(int matterId, IFormFile file, string status)

        {

            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            var document = new

            {

                MatterId = matterId,

                FileName = file.FileName,

                ContentType = file.ContentType,

                FileContent = memoryStream.ToArray(),

                Status = status

            };

            var content = new StringContent(

                JsonSerializer.Serialize(document),

                Encoding.UTF8,

                "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/document", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Document>(responseContent, new JsonSerializerOptions

            {

                PropertyNameCaseInsensitive = true

            });

        }

        public async Task<Document> UpdateDocumentStatusAsync(int documentId, string status)

        {

            var content = new StringContent(

                JsonSerializer.Serialize(status),

                Encoding.UTF8,

                "application/json");

            var response = await _httpClient.PatchAsync($"{_apiBaseUrl}/api/document/{documentId}/status", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)

                return null;

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Document>(responseContent, new JsonSerializerOptions

            {

                PropertyNameCaseInsensitive = true

            });

        }

        public async Task<bool> DeleteDocumentAsync(int documentId)

        {

            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/document/{documentId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)

                return false;

            response.EnsureSuccessStatusCode();

            return true;

        }

        public async Task<byte[]> DownloadDocumentAsync(int documentId)

        {

            var document = await GetDocumentByIdAsync(documentId);

            if (document == null)

                return null;

            // The document content is retrieved from the API

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/document/{documentId}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var fullDocument = JsonSerializer.Deserialize<dynamic>(content, new JsonSerializerOptions

            {

                PropertyNameCaseInsensitive = true

            });

            // Extract the file content as byte array

            return Convert.FromBase64String(fullDocument.GetProperty("fileContent").GetString());

        }

    }

}


 