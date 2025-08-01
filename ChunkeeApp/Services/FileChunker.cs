using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Helpers;
using ChunkeeApp.Interfaces;
using ChunkeeApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChunkeeApp.Services
{
    public class FileChunker : IFileChunker
    {
        private readonly ILogger<FileChunker> _logger;
        private readonly IChunkeeDbContext _dbContext;

        public FileChunker(ILogger<FileChunker> logger, IChunkeeDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task ChunkFile(string filePath, int chunkSizeInBytes)
        {
            _logger.LogInformation("Dosya chunk işlemi başlatıldı: {filePath}", filePath);

            if (!File.Exists(filePath))
            {
                _logger.LogError("Dosya bulunamadı: {filePath}", filePath);
                return;
            }
            
            try
            {
                byte[] buffer = new byte[chunkSizeInBytes];
                int chunkIndex = 0;

                
                string fileHash = FileVerificationHelper.CalculateFileHash(filePath);
                long totalSize = new FileInfo(filePath).Length;

                var originalFile = new OriginalFile
                {
                    FileName = Path.GetFileName(filePath),
                    FileHash = fileHash,
                    TotalSize = totalSize,
                    Chunks = new List<Chunk>()
                };

                int bytesRead;
                using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    byte[] chunkData = buffer.Take(bytesRead).ToArray();

                    var chunk = new Chunk
                    {
                        PartNumber = chunkIndex,
                        Data = chunkData,
                        FileId = originalFile.Id,
                        OriginalFile = originalFile
                    };
                    originalFile.Chunks.Add(chunk);
                    string chunkFileName = $"{filePath}.chunk{chunkIndex}";
                    File.WriteAllBytes(chunkFileName, chunkData);

                    _logger.LogInformation("Chunk {index} yazıldı: {chunkFileName} ", chunkIndex, chunkFileName);

                    chunkIndex++;


                }
                originalFile.TotalChunks = chunkIndex;

                await _dbContext.OriginalFile.AddAsync(originalFile);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Veritabanına kaydedildi: {fileName}, {chunkCount} parça", originalFile.FileName, chunkIndex);


                _logger.LogInformation("Dosya chunk işlemi tamamlandı. Toplam {chunkCount} parça oluşturuldu.", chunkIndex);
            } catch (Exception ex)
            { _logger.LogError(ex, "İşlem sırasında hata oluştu."); }
            

            
        }

       
    }
}
