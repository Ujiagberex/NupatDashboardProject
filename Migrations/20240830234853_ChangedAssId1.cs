using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NupatDashboardProject.Migrations
{
    public partial class ChangedAssId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Assignments",
                newName: "AssignmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "Assignments",
                newName: "Id");
        }
    }
}
