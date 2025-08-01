using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChunkeeApp.Interfaces
{
    public interface IChunkeeDbContext
    {
        DbSet<OriginalFile> OriginalFile { get; set; }
        DbSet<Chunk> Chunks { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
