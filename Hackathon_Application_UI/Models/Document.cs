using System.ComponentModel.DataAnnotations;

namespace Hackathon_Application_UI.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        [Required]
        public int MatterId { get; set; }
        [Required]
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        [Display(Name = "Content Type")]
        public string ContentType { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Last Modified Date")]
        public DateTime LastModifiedDate { get; set; }
    }

    public class DocumentUploadModel
    {
        public int MatterId { get; set; }
        [Required]
        [Display(Name = "Document")]
        public IFormFile File { get; set; }
        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }
    }

    public class DocumentStatusUpdateModel
    {
        public int DocumentId { get; set; }
        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}
