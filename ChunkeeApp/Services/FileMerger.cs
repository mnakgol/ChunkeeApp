using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Data.Database;
using ChunkeeApp.Helpers;
using ChunkeeApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChunkeeApp.Services
{
    public class FileMerger : IFileMerger
    {
        private readonly ILogger<FileMerger> _logger;
        private readonly ChunkeeDbContext _context;

        public FileMerger(ILogger<FileMerger> logger, ChunkeeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

      

        public async Task MergeChunksAsync(string firstChunkPath, string outputFilePath)
        {
            var baseFilePath = Path.GetDirectoryName(firstChunkPath)!;
            var baseFileName = Path.GetFileNameWithoutExtension(firstChunkPath).Split(".chunk")[0];

            var chunkFiles = Directory.GetFiles(baseFilePath, $"{baseFileName}.chunk*")
                                      .OrderBy(f => int.Parse(f.Split(".chunk")[1]))
                                      .ToList();

            if (!chunkFiles.Any())
            {
                _logger.LogWarning("Birleştirme için chunk dosyası bulunamadı.");
                return;
            }

            _logger.LogInformation("Toplam {Count} chunk bulundu.", chunkFiles.Count);

            
            //await using var outputStream = new FileStream(outputFilePath, FileMode.Create);
            _logger.LogInformation("Chunk birleştirme işlemi başlatıldı...");
            
            using (var outputStream = new FileStream(outputFilePath, FileMode.Create))
            {
                foreach (var chunk in chunkFiles)
                {
                    _logger.LogInformation("İşlenecek chunk: {Chunk}", chunk);
                    try
                    {
                        var buffer = await File.ReadAllBytesAsync(chunk);
                        _logger.LogInformation("Okunan byte sayısı: {Length}", buffer.Length);
                        await outputStream.WriteAsync(buffer, 0, buffer.Length);
                        _logger.LogInformation("Chunk eklendi: {Chunk}", chunk);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Chunk işlenirken hata oluştu: {Chunk}", chunk);
                    }
                }
            }
               

            _logger.LogInformation("Chunk birleştirme işlemi tamamlandı: {Output}", outputFilePath);

            try
            {

                string mergedFileHash = FileVerificationHelper.CalculateFileHash(outputFilePath);
                _logger.LogInformation("Birleştirilen dosyanın SHA256 hash'i: {Hash}", mergedFileHash);

                string fileName = Path.GetFileName(firstChunkPath).Substring(0, Path.GetFileName(firstChunkPath).LastIndexOf(".chunk"));

                var originalFile = await _context.OriginalFile
                    .FirstOrDefaultAsync(f => f.FileName == fileName);

                if (originalFile == null)
                {
                    _logger.LogWarning("Veritabanında dosya kaydı bulunamadı: {FileName}", fileName);
                    return;
                }

                if (string.Equals(originalFile.FileHash, mergedFileHash, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Hash doğrulaması başarılı: Dosya bütünlüğü korunmuş.");
                }
                else
                {
                    _logger.LogError("Hash doğrulaması başarısız: Dosya bozulmuş olabilir!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hash hesaplanırken hata oluştu.");
            }

            



        }
    }
}
