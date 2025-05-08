using System;
using System.ComponentModel.DataAnnotations;

namespace Hackathon_Application_Database.Models
{
    public class Document
    {
        public int DocumentId { get; set; }

        [Required]
        public int MatterId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public byte[] FileContent { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
