using Microsoft.EntityFrameworkCore.Migrations;

namespace KeepFitStore.Data.Migrations
{
    public partial class TwoFieldsMore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverFullName",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverPhoneNumber",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverFullName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiverPhoneNumber",
                table: "Orders");
        }
    }
}
