using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ChunkeeApp.Services;
using ChunkeeApp.Data;
using ChunkeeApp.Data.Database;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.EntityFrameworkCore;
using ChunkeeApp.Models;


namespace ChunkeeApp.Tests
{
    public class FileMergerTests
    {
        [Fact]
        public async Task MergeChunksAsync_CreatesMergedFileSuccessfully()
        {
            
            var loggerMock = new Mock<ILogger<FileMerger>>();
            var options = new DbContextOptionsBuilder<ChunkeeDbContext>()
                              .UseInMemoryDatabase(databaseName: "TestDb")
                              .Options;
            using var dbContext = new ChunkeeDbContext(options);

           
            dbContext.OriginalFile.Add(new OriginalFile
            {
                FileName = "file",
                FileHash = "FAKE_HASH_FOR_TEST"
            });
            await dbContext.SaveChangesAsync();

            var fileMerger = new FileMerger(loggerMock.Object, dbContext);

            string basePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(basePath);

           
            string chunk0 = Path.Combine(basePath, "file.chunk0");
            string chunk1 = Path.Combine(basePath, "file.chunk1");

            await File.WriteAllTextAsync(chunk0, "chunk0data");
            await File.WriteAllTextAsync(chunk1, "chunk1data");

            string outputFilePath = Path.Combine(basePath, "merged_file.dat");

            // Act
            await fileMerger.MergeChunksAsync(chunk0, outputFilePath);

            // Assert
            Assert.True(File.Exists(outputFilePath));

            // Cleanup
            Directory.Delete(basePath, true);
        }
    }
}
