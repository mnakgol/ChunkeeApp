using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkeeApp.Interfaces
{
    public interface IFileMerger
    {
        Task MergeChunksAsync(string firstChunkPath, string outputFilePath);
    }
}
