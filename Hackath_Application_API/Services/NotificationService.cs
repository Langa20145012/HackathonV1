
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Hackath_Application_API.Interfaces;
using Hackathon_Application_Database.DatabaseContext;
using Hackathon_Application_Database.Models;


namespace Hackath_Application_API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly string _matterServiceUrl;

        public NotificationService(
            ApplicationDbContext dbContext,
            IEmailService emailService,
            HttpClient httpClient,
            ILogger<NotificationService> logger,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _httpClient = httpClient;
            _logger = logger;
            _matterServiceUrl = configuration["ServiceUrls:MatterService"];
        }

        public async Task<bool> ProcessDocumentStatusChangeAsync(NotificationRequest request)
        {
            try
            {
                // Get matter details to retrieve client email
                var matterResponse = await _httpClient.GetAsync($"{_matterServiceUrl}/api/matter/{request.MatterId}");

                if (!matterResponse.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve matter details for MatterId: {request.MatterId}");
                    return false;
                }

                var matterJson = await matterResponse.Content.ReadAsStringAsync();
                var matter = JsonSerializer.Deserialize<dynamic>(matterJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                string clientEmail = matter.GetProperty("clientEmail").GetString();

                // Prepare email content
                string subject = $"Document Status Update: {request.FileName}";
                string body = $@"
                    <html>
                    <body>
                        <h2>Document Status Update</h2>
                        <p>The status of document <strong>{request.FileName}</strong> has been updated to <strong>{request.Status}</strong>.</p>
                        <p>Matter ID: {request.MatterId}</p>
                        <p>Document ID: {request.DocumentId}</p>
                        <p>Thank you for using our services.</p>
                    </body>
                    </html>
                ";

                // Send email
                bool emailSent = await _emailService.SendEmailAsync(clientEmail, subject, body);

                // Log notification
                var notificationLog = new NotificationLog
                {
                    DocumentId = request.DocumentId,
                    MatterId = request.MatterId,
                    EmailTo = clientEmail,
                    Subject = subject,
                    Message = body,
                    SentDate = DateTime.UtcNow,
                    Status = emailSent ? "Sent" : "Failed"
                };

                _dbContext.NotificationLogs.Add(notificationLog);
                await _dbContext.SaveChangesAsync();

                return emailSent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing document status change notification for DocumentId: {request.DocumentId}");
                return false;
            }
        }
    }
}
