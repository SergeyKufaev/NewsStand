using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsStand.Migrations
{
    public partial class RemovePurchaseFromPurchaseProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseProducts",
                table: "PurchaseProducts");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseId",
                table: "PurchaseProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PurchaseProducts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseProducts",
                table: "PurchaseProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseProducts_PurchaseId",
                table: "PurchaseProducts",
                column: "PurchaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseProducts",
                table: "PurchaseProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseProducts_PurchaseId",
                table: "PurchaseProducts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PurchaseProducts");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseId",
                table: "PurchaseProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseProducts",
                table: "PurchaseProducts",
                columns: new[] { "PurchaseId", "ProductId" });
        }
    }
}
