using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSolutions.web.Migrations
{
    /// <inheritdoc />
    public partial class Org_Add_AddMerchantStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedStaffId",
                table: "MerchantDecisions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedStaffId",
                table: "MerchantDecisions");
        }
    }
}
