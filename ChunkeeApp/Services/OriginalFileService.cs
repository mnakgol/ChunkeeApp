using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Interfaces;
using ChunkeeApp.Models;

namespace ChunkeeApp.Services
{
    public class OriginalFileService : IOriginalFileService
    {
        private readonly IRepository<OriginalFile> _fileRepository;

        public OriginalFileService(IRepository<OriginalFile> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<IEnumerable<OriginalFile>> GetAllFilesAsync()
        {
            return await _fileRepository.GetAllAsync();
        }

        public async Task<OriginalFile?> GetFileByIdAsync(Guid id)
        {
            return await _fileRepository.GetByIdAsync(id);
        }

        public async Task AddFileAsync(OriginalFile file)
        {
            await _fileRepository.AddAsync(file);
            await _fileRepository.SaveChangesAsync();
        }

        public async Task UpdateFileAsync(OriginalFile file)
        {
            _fileRepository.Update(file);
            await _fileRepository.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(Guid id)
        {
            var file = await _fileRepository.GetByIdAsync(id);
            if (file is not null)
            {
                _fileRepository.Remove(file);
                await _fileRepository.SaveChangesAsync();
            }
        }
    }
}
