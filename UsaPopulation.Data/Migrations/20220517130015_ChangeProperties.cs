using Microsoft.EntityFrameworkCore.Migrations;

namespace UsaPopulation.Data.Migrations
{
    public partial class ChangeProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "QueryLogs",
                newName: "PathAndQuery");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PathAndQuery",
                table: "QueryLogs",
                newName: "Url");
        }
    }
}
