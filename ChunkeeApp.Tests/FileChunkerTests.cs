using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Data.Database;
using ChunkeeApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ChunkeeApp.Tests
{
    public class FileChunkerTests
    {
        [Fact]
        public async Task FileChunker_ChunkFile_CreatesChunksSuccessfully()
        {
            
            var loggerMock = new Mock<ILogger<FileChunker>>();
            var options = new DbContextOptionsBuilder<ChunkeeDbContext>()
                              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                              .Options;
            using var dbContext = new ChunkeeDbContext(options);

            var fileChunker = new FileChunker(loggerMock.Object, dbContext);

            
            string basePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(basePath);

            string testFilePath = Path.Combine(basePath, "testfile.txt");
            byte[] fileContent = Encoding.UTF8.GetBytes("Bu bir test dosyasıdır. Chunk işlemi yapılacak.");
            await File.WriteAllBytesAsync(testFilePath, fileContent);

            int chunkSize = 10; // Her chunk en fazla 10 byte

          
            await fileChunker.ChunkFile(testFilePath, chunkSize);

            
            var chunkFiles = Directory.GetFiles(basePath, "*.chunk*");
            Assert.NotEmpty(chunkFiles);

           
            int expectedChunkCount = (int)Math.Ceiling((double)fileContent.Length / chunkSize);
            Assert.Equal(expectedChunkCount, chunkFiles.Length);

          
            var originalFile = await dbContext.OriginalFile
                .Include(f => f.Chunks)
                .FirstOrDefaultAsync(f => f.FileName == "testfile.txt");

            Assert.NotNull(originalFile);
            Assert.Equal(expectedChunkCount, originalFile.TotalChunks);
            Assert.Equal(fileContent.Length, originalFile.TotalSize);
            Assert.NotEmpty(originalFile.Chunks);
            Assert.Equal(expectedChunkCount, originalFile.Chunks.Count);

          
            for (int i = 0; i < expectedChunkCount; i++)
            {
                string chunkPath = $"{testFilePath}.chunk{i}";
                Assert.True(File.Exists(chunkPath));
                byte[] chunkData = await File.ReadAllBytesAsync(chunkPath);

                int expectedLength = (i < expectedChunkCount - 1) ? chunkSize : fileContent.Length - (chunkSize * i);
                Assert.Equal(expectedLength, chunkData.Length);

                byte[] expectedData = fileContent.Skip(i * chunkSize).Take(expectedLength).ToArray();
                Assert.Equal(expectedData, chunkData);
            }

            // Cleanup
            Directory.Delete(basePath, true);
        }
    }
}
