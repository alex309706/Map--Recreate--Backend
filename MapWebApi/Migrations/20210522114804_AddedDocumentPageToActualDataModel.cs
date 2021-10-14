using Microsoft.EntityFrameworkCore.Migrations;

namespace MapWebApi.Migrations
{
    public partial class AddedDocumentPageToActualDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentPage",
                table: "ActualData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentPage",
                table: "ActualData");
        }
    }
}
