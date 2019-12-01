using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaycareSolutionSystem.Database.DataContext.Migrations
{
    public partial class ImageSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "RegisteredClientAction",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfilePictureId",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfilePictureId",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BinaryData = table.Column<byte[]>(nullable: true),
                    MimeType = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredClientAction_PhotoId",
                table: "RegisteredClientAction",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ProfilePictureId",
                table: "Employee",
                column: "ProfilePictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ProfilePictureId",
                table: "Client",
                column: "ProfilePictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Picture_ProfilePictureId",
                table: "Client",
                column: "ProfilePictureId",
                principalTable: "Picture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Picture_ProfilePictureId",
                table: "Employee",
                column: "ProfilePictureId",
                principalTable: "Picture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredClientAction_Picture_PhotoId",
                table: "RegisteredClientAction",
                column: "PhotoId",
                principalTable: "Picture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Picture_ProfilePictureId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Picture_ProfilePictureId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredClientAction_Picture_PhotoId",
                table: "RegisteredClientAction");

            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.DropIndex(
                name: "IX_RegisteredClientAction_PhotoId",
                table: "RegisteredClientAction");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ProfilePictureId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Client_ProfilePictureId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "RegisteredClientAction");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "Client");
        }
    }
}
