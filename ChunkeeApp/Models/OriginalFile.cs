using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkeeApp.Models
{
    public class OriginalFile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        public long TotalSize { get; set; }

        [Required]
        public int TotalChunks { get; set; }

        [MaxLength(128)]
        public string FileHash { get; set; }  // SHA-256 hash

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

       

        // Navigation Property
        public virtual ICollection<Chunk> Chunks { get; set; }
    }
}
