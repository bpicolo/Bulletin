using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bulletin.Models
{
    public class Attachment : IAttachment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Board { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public string OriginalFilename { get; set; }

        [Required]
        public string Checksum { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public long SizeInBytes { get; set; }

        public DateTime? DeletedAt { get; set; } = null;

        public string Metadata { get; set; }
    }
}
