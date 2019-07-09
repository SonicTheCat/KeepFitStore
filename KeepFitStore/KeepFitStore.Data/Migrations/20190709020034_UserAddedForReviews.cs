using Microsoft.EntityFrameworkCore.Migrations;

namespace KeepFitStore.Data.Migrations
{
    public partial class UserAddedForReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeepFitUserId",
                table: "Reviews",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_KeepFitUserId",
                table: "Reviews",
                column: "KeepFitUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_KeepFitUserId",
                table: "Reviews",
                column: "KeepFitUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_KeepFitUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_KeepFitUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "KeepFitUserId",
                table: "Reviews");
        }
    }
}
