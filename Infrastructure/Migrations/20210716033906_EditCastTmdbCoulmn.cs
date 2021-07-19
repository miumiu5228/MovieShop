using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class EditCastTmdbCoulmn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TmbdUrl",
                table: "Cast",
                newName: "TmdbUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TmdbUrl",
                table: "Cast",
                newName: "TmbdUrl");
        }
    }
}
