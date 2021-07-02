using Microsoft.EntityFrameworkCore.Migrations;

namespace Oz.Data.Migrations
{
    public partial class AddedApprovedC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Accounts");
        }
    }
}
