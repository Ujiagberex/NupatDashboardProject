using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NupatDashboardProject.Migrations
{
    public partial class Changepassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordChanged",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPasswordChanged",
                table: "AspNetUsers");
        }
    }
}
