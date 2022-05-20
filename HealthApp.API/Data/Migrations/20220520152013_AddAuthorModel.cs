using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthApp.API.Data.Migrations
{
    public partial class AddAuthorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publication",
                table: "Records");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Records",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Records",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Categories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_AuthorId",
                table: "Records",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Authors_AuthorId",
                table: "Records",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Authors_AuthorId",
                table: "Records");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Records_AuthorId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Publication",
                table: "Records",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
