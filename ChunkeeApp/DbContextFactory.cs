using ChunkeeApp.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ChunkeeApp
{
    public class ChunkeeDbContextFactory : IDesignTimeDbContextFactory<ChunkeeDbContext>
    {
        public ChunkeeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChunkeeDbContext>();
            optionsBuilder.UseSqlite("Data Source=chunkee.db");

            return new ChunkeeDbContext(optionsBuilder.Options);
        }
    }
}
