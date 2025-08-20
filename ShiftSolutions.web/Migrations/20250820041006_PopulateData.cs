using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSolutions.web.Migrations
{
    /// <inheritdoc />
    public partial class PopulateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApartmentsOnLine",
                table: "ApartmentsOnLine");

            migrationBuilder.RenameTable(
                name: "ApartmentsOnLine",
                newName: "Apartments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Apartments",
                table: "Apartments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MerchantDecisions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ByUserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ByIp = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    AffectedApartments = table.Column<int>(type: "int", nullable: false),
                    MetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantDecisions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantDecisions_AgentId",
                table: "MerchantDecisions",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantDecisions_AgentId_AtUtc",
                table: "MerchantDecisions",
                columns: new[] { "AgentId", "AtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantDecisions_AtUtc",
                table: "MerchantDecisions",
                column: "AtUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MerchantDecisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Apartments",
                table: "Apartments");

            migrationBuilder.RenameTable(
                name: "Apartments",
                newName: "ApartmentsOnLine");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApartmentsOnLine",
                table: "ApartmentsOnLine",
                column: "Id");
        }
    }
}
