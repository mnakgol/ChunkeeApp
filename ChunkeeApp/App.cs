using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Interfaces;
using ChunkeeApp.Services;
using Microsoft.Extensions.Logging;

namespace ChunkeeApp
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IFileChunker _fileChunker;
        private readonly IFileMerger _fileMerger;

        public App(ILogger<App> logger, IFileChunker fileChunker,IFileMerger fileMerger)
        {
            _logger = logger;
            _fileChunker = fileChunker;
            _fileMerger = fileMerger;
        }

        

       

        public async Task Run()
        {
            _logger.LogInformation("Uygulama başlatıldı.");

            Console.Write("İşlem türünü seçin (1: Chunk, 2: Merge): ");
            var choice = Console.ReadLine();


            if (choice == "1")
            {
                Console.Write("Parçalanacak dosyanın tam yolunu girin: ");
                string? filePath = Console.ReadLine();

                Console.Write("Parça boyutunu byte cinsinden girin (örneğin 1024): ");
                if (!int.TryParse(Console.ReadLine(), out int chunkSize))
                {
                    Console.WriteLine("Geçersiz parça boyutu.");
                    return;
                }

                Console.WriteLine("Dosya chunk işlemleri başlatılıyor...");
                 _fileChunker.ChunkFile(filePath, chunkSize);
            }
            else if (choice == "2")
            {
                Console.Write("İlk chunk dosyasının tam yolunu girin: ");
                var firstChunkPath = Console.ReadLine();

                Console.Write("Birleştirilmiş dosya için çıktı dosya yolunu girin: ");
                var outputPath = Console.ReadLine();

                await _fileMerger.MergeChunksAsync(firstChunkPath!, outputPath!);
            }
            else
            {
                Console.WriteLine("Geçersiz seçim!");
            }




            _logger.LogInformation("Uygulama başarıyla tamamlandı.");
        }
    }
}
