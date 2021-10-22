using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsStand.Migrations
{
    public partial class ApplyCascadeDeleteOnPurchaseProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseProducts_Purchases_PurchaseId",
                table: "PurchaseProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseProducts_Purchases_PurchaseId",
                table: "PurchaseProducts",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseProducts_Purchases_PurchaseId",
                table: "PurchaseProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseProducts_Purchases_PurchaseId",
                table: "PurchaseProducts",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id");
        }
    }
}
