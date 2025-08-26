using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSolutions.web.Migrations
{
    /// <inheritdoc />
    public partial class Bookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    ApartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApartmentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApartmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApartmentCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApartmentPricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApartmentStarRatings = table.Column<int>(type: "int", nullable: false),
                    ApartmentStayDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfNights = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VatPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
