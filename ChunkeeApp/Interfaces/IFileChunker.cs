using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkeeApp.Interfaces
{
    public interface IFileChunker
    {
        public Task ChunkFile(string filePath, int chunkSizeInBytes);
    }
}
