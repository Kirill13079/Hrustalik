using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthApp.API.Data.Migrations
{
    public partial class UpdateRecordModelArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHotDeal",
                table: "Records");

            migrationBuilder.AddColumn<bool>(
                name: "IsArticle",
                table: "Records",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHot",
                table: "Records",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArticle",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "IsHot",
                table: "Records");

            migrationBuilder.AddColumn<bool>(
                name: "IsHotDeal",
                table: "Records",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
