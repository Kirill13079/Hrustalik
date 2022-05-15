using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthApp.API.Data.Migrations
{
    public partial class AddYoutubeRecoredModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsYoutube",
                table: "Records",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsYoutube",
                table: "Records");
        }
    }
}
