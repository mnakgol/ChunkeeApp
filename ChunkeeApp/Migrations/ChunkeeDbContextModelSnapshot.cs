﻿// <auto-generated />
using System;
using ChunkeeApp.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChunkeeApp.Migrations
{
    [DbContext(typeof(ChunkeeDbContext))]
    partial class ChunkeeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.7");

            modelBuilder.Entity("ChunkeeApp.Models.Chunk", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<Guid>("FileId")
                        .HasColumnType("TEXT");

                    b.Property<int>("PartNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("Chunks");
                });

            modelBuilder.Entity("ChunkeeApp.Models.OriginalFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalChunks")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TotalSize")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("OriginalFile");
                });

            modelBuilder.Entity("ChunkeeApp.Models.Chunk", b =>
                {
                    b.HasOne("ChunkeeApp.Models.OriginalFile", "OriginalFile")
                        .WithMany("Chunks")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OriginalFile");
                });

            modelBuilder.Entity("ChunkeeApp.Models.OriginalFile", b =>
                {
                    b.Navigation("Chunks");
                });
#pragma warning restore 612, 618
        }
    }
}
