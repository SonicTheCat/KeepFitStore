using Microsoft.EntityFrameworkCore.Migrations;

namespace KeepFitStore.Data.Migrations
{
    public partial class RemovePropsFromAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingNumebr",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Addresses");

            migrationBuilder.AlterColumn<int>(
                name: "StreetNumber",
                table: "Addresses",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StreetNumber",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "BuildingNumebr",
                table: "Addresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Addresses",
                nullable: true);
        }
    }
}
