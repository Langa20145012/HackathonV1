using System.ComponentModel.DataAnnotations;

namespace Hackathon_Application_UI.Models
{
    public class Matter
    {
        public int MatterId { get; set; }
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Client ID")]
        public int ClientId { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Client Email")]
        public string ClientEmail { get; set; }
        [Required]
        [Display(Name = "Status")]
        public int Status { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Last Modified Date")]
        public DateTime LastModifiedDate { get; set; }

        public string Participant { get; set; }

        public string Name { get; set; }
    }
}