using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab10.Migrations
{
    public partial class updateArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Articles",
                newName: "ImagePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Articles",
                newName: "Image");
        }
    }
}
