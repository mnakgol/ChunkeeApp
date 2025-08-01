using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Interfaces;
using ChunkeeApp.Models;

namespace ChunkeeApp.Services
{
    public class ChunkService : IChunkService
    {
        private readonly IRepository<Chunk> _chunkRepository;

        public ChunkService(IRepository<Chunk> chunkRepository)
        {
            _chunkRepository = chunkRepository;
        }

        public async Task<IEnumerable<Chunk>> GetAllChunksAsync()
        {
            return await _chunkRepository.GetAllAsync();
        }

        public async Task<Chunk?> GetChunkByIdAsync(Guid id)
        {
            return await _chunkRepository.GetByIdAsync(id);
        }

        public async Task AddChunkAsync(Chunk chunk)
        {
            await _chunkRepository.AddAsync(chunk);
            await _chunkRepository.SaveChangesAsync();
        }

        public async Task UpdateChunkAsync(Chunk chunk)
        {
            _chunkRepository.Update(chunk);
            await _chunkRepository.SaveChangesAsync();
        }

        public async Task DeleteChunkAsync(Guid id)
        {
            var chunk = await _chunkRepository.GetByIdAsync(id);
            if (chunk is not null)
            {
                _chunkRepository.Remove(chunk);
                await _chunkRepository.SaveChangesAsync();
            }
        }
    }
}
