using Microsoft.EntityFrameworkCore.Migrations;

namespace Oz.Data.Migrations
{
    public partial class UpdateProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ColorNotApplicable",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SizeNotApplicable",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorNotApplicable",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SizeNotApplicable",
                table: "Products");
        }
    }
}
