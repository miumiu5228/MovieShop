using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdatingCastTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TumbdUrl",
                table: "Cast",
                newName: "TmbdUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TmbdUrl",
                table: "Cast",
                newName: "TumbdUrl");
        }
    }
}
