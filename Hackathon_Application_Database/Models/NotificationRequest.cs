using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon_Application_Database.Models
{
    public class NotificationRequest
    {
        [Required]
        public int DocumentId { get; set; }

        [Required]
        public int MatterId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
