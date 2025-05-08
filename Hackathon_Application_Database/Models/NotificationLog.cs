using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon_Application_Database.Models
{
    public class NotificationLog
    {
        public int NotificationId { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [Required]
        public int MatterId { get; set; }

        [Required]
        [EmailAddress]
        public string EmailTo { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime SentDate { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
