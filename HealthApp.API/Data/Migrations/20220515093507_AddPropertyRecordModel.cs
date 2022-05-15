using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthApp.API.Data.Migrations
{
    public partial class AddPropertyRecordModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "Records",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TextXAML",
                table: "Records",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "TextXAML",
                table: "Records");
        }
    }
}
