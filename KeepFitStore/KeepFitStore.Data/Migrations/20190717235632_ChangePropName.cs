using Microsoft.EntityFrameworkCore.Migrations;

namespace KeepFitStore.Data.Migrations
{
    public partial class ChangePropName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductQiantity",
                table: "ProductOrders",
                newName: "ProductQuantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductQuantity",
                table: "ProductOrders",
                newName: "ProductQiantity");
        }
    }
}
