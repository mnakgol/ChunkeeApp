using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChunkeeApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OriginalFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    TotalSize = table.Column<long>(type: "INTEGER", nullable: false),
                    TotalChunks = table.Column<int>(type: "INTEGER", nullable: false),
                    FileHash = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chunks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PartNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false),
                    FileId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chunks_OriginalFile_FileId",
                        column: x => x.FileId,
                        principalTable: "OriginalFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chunks_FileId",
                table: "Chunks",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chunks");

            migrationBuilder.DropTable(
                name: "OriginalFile");
        }
    }
}
