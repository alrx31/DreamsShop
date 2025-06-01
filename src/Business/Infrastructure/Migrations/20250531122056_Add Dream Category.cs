using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDreamCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Dream_DreamId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_DreamId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DreamId",
                table: "Category");

            migrationBuilder.CreateTable(
                name: "DreamCategory",
                columns: table => new
                {
                    DreamCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Dreams = table.Column<Guid>(type: "uuid", nullable: false),
                    Categories = table.Column<Guid>(type: "uuid", nullable: false),
                    DreamId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DreamCategory", x => x.DreamCategoryId);
                    table.ForeignKey(
                        name: "FK_DreamCategory_Dream_DreamId",
                        column: x => x.DreamId,
                        principalTable: "Dream",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DreamCategory_DreamId",
                table: "DreamCategory",
                column: "DreamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DreamCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "DreamId",
                table: "Category",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_DreamId",
                table: "Category",
                column: "DreamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Dream_DreamId",
                table: "Category",
                column: "DreamId",
                principalTable: "Dream",
                principalColumn: "Id");
        }
    }
}
