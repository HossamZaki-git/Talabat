using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    public partial class Order_OrderItem_Cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_Orders_OrderID",
                table: "OrderedItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_Orders_OrderID",
                table: "OrderedItems",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_Orders_OrderID",
                table: "OrderedItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_Orders_OrderID",
                table: "OrderedItems",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID");
        }
    }
}
