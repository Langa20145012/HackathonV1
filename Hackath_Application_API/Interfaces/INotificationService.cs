using Hackathon_Application_Database.Models;

namespace Hackath_Application_API.Interfaces
{
    public interface INotificationService
    {
        Task<bool> ProcessDocumentStatusChangeAsync(NotificationRequest request);
    }
}
