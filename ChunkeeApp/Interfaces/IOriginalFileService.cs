using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Models;

namespace ChunkeeApp.Interfaces
{
    public interface IOriginalFileService
    {
        Task<IEnumerable<OriginalFile>> GetAllFilesAsync();
        Task<OriginalFile?> GetFileByIdAsync(Guid id);
        Task AddFileAsync(OriginalFile file);
        Task UpdateFileAsync(OriginalFile file);
        Task DeleteFileAsync(Guid id);
    }
}
