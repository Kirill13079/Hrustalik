using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthApp.API.Data.Migrations
{
    public partial class RemoveSmallSizeRecordModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmallSize",
                table: "Records");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SmallSize",
                table: "Records",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
