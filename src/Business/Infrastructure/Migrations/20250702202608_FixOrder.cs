using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Dream_DreamId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DreamId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DreamId",
                table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "DreamId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DreamId",
                table: "Orders",
                column: "DreamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Dream_DreamId",
                table: "Orders",
                column: "DreamId",
                principalTable: "Dream",
                principalColumn: "DreamId");
        }
    }
}
