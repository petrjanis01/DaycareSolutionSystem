using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaycareSolutionSystem.Database.DataContext.Migrations
{
    public partial class RelationAndNamingChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Address_AddressId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "EstimatedDuration",
                table: "AgreedClientAction");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginName",
                table: "User",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PlannedStartTime",
                table: "RegisteredClientAction",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Client",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedDurationMinutes",
                table: "AgreedClientAction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PlannedStartTime",
                table: "AgreedClientAction",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_User_LoginName",
                table: "User",
                column: "LoginName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Address_AddressId",
                table: "Client",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Address_AddressId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_User_LoginName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PlannedStartTime",
                table: "RegisteredClientAction");

            migrationBuilder.DropColumn(
                name: "EstimatedDurationMinutes",
                table: "AgreedClientAction");

            migrationBuilder.DropColumn(
                name: "PlannedStartTime",
                table: "AgreedClientAction");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginName",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Client",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<int>(
                name: "EstimatedDuration",
                table: "AgreedClientAction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Address_AddressId",
                table: "Client",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
