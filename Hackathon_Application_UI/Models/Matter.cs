using System.ComponentModel.DataAnnotations;

namespace Hackathon_Application_UI.Models
{
    public class Matter
    {
        public int MatterId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        [EmailAddress]
        public string ClientEmail { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
