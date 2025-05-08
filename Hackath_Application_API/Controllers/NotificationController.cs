using Hackath_Application_API.Interfaces;
using Hackathon_Application_Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackath_Application_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("document-status-changed")]
        public async Task<IActionResult> DocumentStatusChanged([FromBody] NotificationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _notificationService.ProcessDocumentStatusChangeAsync(request);

            if (result)
                return Ok(new { success = true, message = "Notification sent successfully" });
            else
                return StatusCode(500, new { success = false, message = "Failed to send notification" });
        }
    }
}
