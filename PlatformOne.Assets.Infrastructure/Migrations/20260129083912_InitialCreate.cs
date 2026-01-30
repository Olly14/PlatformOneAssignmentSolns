using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformOne.Assets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Isin = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SourceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PriceDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LastUpdatedUtc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetPrices_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetPrices_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetPrices_AssetId_SourceId_PriceDate",
                table: "AssetPrices",
                columns: new[] { "AssetId", "SourceId", "PriceDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetPrices_SourceId",
                table: "AssetPrices",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Isin",
                table: "Assets",
                column: "Isin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Symbol",
                table: "Assets",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sources_Name",
                table: "Sources",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetPrices");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
