using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaycareSolutionSystem.Database.DataContext.Migrations
{
    public partial class ActionStartEndDateTimeAsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedStartTime",
                table: "RegisteredClientAction");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionStartedDateTime",
                table: "RegisteredClientAction",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionFinishedDateTime",
                table: "RegisteredClientAction",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedStartDateTime",
                table: "RegisteredClientAction",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedStartDateTime",
                table: "RegisteredClientAction");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionStartedDateTime",
                table: "RegisteredClientAction",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionFinishedDateTime",
                table: "RegisteredClientAction",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PlannedStartTime",
                table: "RegisteredClientAction",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
