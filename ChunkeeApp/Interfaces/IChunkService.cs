using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Models;

namespace ChunkeeApp.Interfaces
{
    public interface IChunkService
    {
        Task<IEnumerable<Chunk>> GetAllChunksAsync();
        Task<Chunk?> GetChunkByIdAsync(Guid id);
        Task AddChunkAsync(Chunk chunk);
        Task UpdateChunkAsync(Chunk chunk);
        Task DeleteChunkAsync(Guid id);
    }
}
