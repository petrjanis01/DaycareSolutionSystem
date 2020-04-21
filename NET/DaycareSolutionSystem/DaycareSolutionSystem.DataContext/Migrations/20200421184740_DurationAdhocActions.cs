using Microsoft.EntityFrameworkCore.Migrations;

namespace DaycareSolutionSystem.Database.DataContext.Migrations
{
    public partial class DurationAdhocActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstimatedDurationMinutes",
                table: "RegisteredClientAction",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedDurationMinutes",
                table: "RegisteredClientAction");
        }
    }
}
