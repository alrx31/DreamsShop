using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsumerUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumerUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    File_Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    File_Extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    File_Size = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    File_Path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    File = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Order_Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Raiting = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Consumer_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Transaction_Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_ConsumerUser_Consumer_Id",
                        column: x => x.Consumer_Id,
                        principalTable: "ConsumerUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_OrderTransaction_Transaction_Id",
                        column: x => x.Transaction_Id,
                        principalTable: "OrderTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dream",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Desctiption = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Image_Media_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Preview_Media_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Producer_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Raiting = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dream", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dream_Media_Image_Media_Id",
                        column: x => x.Image_Media_Id,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dream_Media_Preview_Media_Id",
                        column: x => x.Preview_Media_Id,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dream_Producer_Producer_Id",
                        column: x => x.Producer_Id,
                        principalTable: "Producer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProducerUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Producer_Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProducerUser_Producer_Producer_Id",
                        column: x => x.Producer_Id,
                        principalTable: "Producer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaitingsProducer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Producer_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Consumer_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaitingsProducer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaitingsProducer_ConsumerUser_Consumer_Id",
                        column: x => x.Consumer_Id,
                        principalTable: "ConsumerUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaitingsProducer_Producer_Producer_Id",
                        column: x => x.Producer_Id,
                        principalTable: "Producer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DreamInCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Dream_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category_Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DreamInCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DreamInCategory_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DreamInCategory_Dream_Dream_Id",
                        column: x => x.Dream_Id,
                        principalTable: "Dream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DreamInOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Dream_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Order_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AddDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Raiting = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DreamInOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DreamInOrder_Dream_Dream_Id",
                        column: x => x.Dream_Id,
                        principalTable: "Dream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DreamInOrder_Order_Order_Id",
                        column: x => x.Order_Id,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaitingsDreams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Dream_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Consumer_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaitingsDreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaitingsDreams_ConsumerUser_Consumer_Id",
                        column: x => x.Consumer_Id,
                        principalTable: "ConsumerUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaitingsDreams_Dream_Dream_Id",
                        column: x => x.Dream_Id,
                        principalTable: "Dream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dream_Image_Media_Id",
                table: "Dream",
                column: "Image_Media_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dream_Preview_Media_Id",
                table: "Dream",
                column: "Preview_Media_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dream_Producer_Id",
                table: "Dream",
                column: "Producer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DreamInCategory_Category_Id",
                table: "DreamInCategory",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DreamInCategory_Dream_Id",
                table: "DreamInCategory",
                column: "Dream_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DreamInOrder_Dream_Id",
                table: "DreamInOrder",
                column: "Dream_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DreamInOrder_Order_Id",
                table: "DreamInOrder",
                column: "Order_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Consumer_Id",
                table: "Order",
                column: "Consumer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Transaction_Id",
                table: "Order",
                column: "Transaction_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerUser_Producer_Id",
                table: "ProducerUser",
                column: "Producer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RaitingsDreams_Consumer_Id",
                table: "RaitingsDreams",
                column: "Consumer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RaitingsDreams_Dream_Id",
                table: "RaitingsDreams",
                column: "Dream_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RaitingsProducer_Consumer_Id",
                table: "RaitingsProducer",
                column: "Consumer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RaitingsProducer_Producer_Id",
                table: "RaitingsProducer",
                column: "Producer_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DreamInCategory");

            migrationBuilder.DropTable(
                name: "DreamInOrder");

            migrationBuilder.DropTable(
                name: "ProducerUser");

            migrationBuilder.DropTable(
                name: "RaitingsDreams");

            migrationBuilder.DropTable(
                name: "RaitingsProducer");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Dream");

            migrationBuilder.DropTable(
                name: "ConsumerUser");

            migrationBuilder.DropTable(
                name: "OrderTransaction");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Producer");
        }
    }
}
