using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaycareSolutionSystem.Database.DataContext.Migrations
{
    public partial class Action_RegisteredActionLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActionId",
                table: "RegisteredClientAction",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredClientAction_ActionId",
                table: "RegisteredClientAction",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredClientAction_Action_ActionId",
                table: "RegisteredClientAction",
                column: "ActionId",
                principalTable: "Action",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredClientAction_Action_ActionId",
                table: "RegisteredClientAction");

            migrationBuilder.DropIndex(
                name: "IX_RegisteredClientAction_ActionId",
                table: "RegisteredClientAction");

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "RegisteredClientAction");
        }
    }
}
