using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DetailsInventoryTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetailsInventories",
                columns: table => new
                {
                    IdDetailsIventory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdInventory = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    LastStock = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsInventories", x => x.IdDetailsIventory);
                    table.ForeignKey(
                        name: "FK_DetailsInventories_Inventories_IdInventory",
                        column: x => x.IdInventory,
                        principalTable: "Inventories",
                        principalColumn: "IdInventory");
                    table.ForeignKey(
                        name: "FK_DetailsInventories_Products_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Products",
                        principalColumn: "IdProduct");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailsInventories_IdInventory",
                table: "DetailsInventories",
                column: "IdInventory");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsInventories_IdProducto",
                table: "DetailsInventories",
                column: "IdProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailsInventories");
        }
    }
}
