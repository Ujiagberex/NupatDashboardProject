using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NupatDashboardProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacilitatorName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacilitatorName",
                table: "Courses");
        }
    }
}
