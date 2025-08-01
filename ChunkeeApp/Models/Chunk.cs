using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkeeApp.Models
{
    public class Chunk
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public int PartNumber { get; set; }
        [Required]
        public byte[] Data { get; set; }
        [Required]
        public Guid FileId { get; set; }

        public virtual OriginalFile OriginalFile { get; set; }

    }
}
