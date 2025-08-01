using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChunkeeApp.Interfaces;
using ChunkeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChunkeeApp.Data.Database
{
    public class ChunkeeDbContext : DbContext,IChunkeeDbContext
    {
        public DbSet<OriginalFile> OriginalFile { get; set; }
        public DbSet<Chunk> Chunks { get; set; }

        public ChunkeeDbContext(DbContextOptions<ChunkeeDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Chunk>()
        .HasOne(c => c.OriginalFile)
        .WithMany(f => f.Chunks)
        .HasForeignKey(c => c.FileId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chunk>()
       .Property(c => c.Data)
       .HasColumnType("BLOB");

            base.OnModelCreating(modelBuilder);
        }
    }
}
