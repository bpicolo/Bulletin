using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Bulletin.Models
{
    [Table(name: "bulletin_attachments")]
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
        public string Filename { get; set; }

        [Required]
        public string Checksum { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public long SizeInBytes { get; set; }

        public string Metadata { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Attachment>()
                .HasIndex(a => a.Location);
        }
    }
}
