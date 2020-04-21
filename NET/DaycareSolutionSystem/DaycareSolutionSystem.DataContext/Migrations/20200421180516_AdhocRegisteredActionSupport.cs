using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaycareSolutionSystem.Database.DataContext.Migrations
{
    public partial class AdhocRegisteredActionSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredClientAction_AgreedClientAction_AgreedClientActio~",
                table: "RegisteredClientAction");

            migrationBuilder.AlterColumn<Guid>(
                name: "AgreedClientActionId",
                table: "RegisteredClientAction",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredClientAction_AgreedClientAction_AgreedClientActio~",
                table: "RegisteredClientAction",
                column: "AgreedClientActionId",
                principalTable: "AgreedClientAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredClientAction_AgreedClientAction_AgreedClientActio~",
                table: "RegisteredClientAction");

            migrationBuilder.AlterColumn<Guid>(
                name: "AgreedClientActionId",
                table: "RegisteredClientAction",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredClientAction_AgreedClientAction_AgreedClientActio~",
                table: "RegisteredClientAction",
                column: "AgreedClientActionId",
                principalTable: "AgreedClientAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
