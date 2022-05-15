using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthApp.API.Data.Migrations
{
    public partial class AddSizeRecordModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SmallSize",
                table: "Records",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmallSize",
                table: "Records");
        }
    }
}
