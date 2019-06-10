using Microsoft.EntityFrameworkCore.Migrations;

namespace KeepFitStore.Data.Migrations
{
    public partial class DeleteFromBasketFk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_AspNetUsers_KeepFitUserId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_KeepFitUserId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "KeepFitUserId",
                table: "Baskets");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BasketId",
                table: "AspNetUsers",
                column: "BasketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Baskets_BasketId",
                table: "AspNetUsers",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Baskets_BasketId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BasketId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "KeepFitUserId",
                table: "Baskets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_KeepFitUserId",
                table: "Baskets",
                column: "KeepFitUserId",
                unique: true,
                filter: "[KeepFitUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_AspNetUsers_KeepFitUserId",
                table: "Baskets",
                column: "KeepFitUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
