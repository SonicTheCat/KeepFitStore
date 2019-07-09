using Microsoft.EntityFrameworkCore.Migrations;

namespace KeepFitStore.Data.Migrations
{
    public partial class addProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AminoAcidProteinPerServing",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AminoAcidSalt",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProteinFibre",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProteinSalt",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AminoAcidProteinPerServing",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AminoAcidSalt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProteinFibre",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProteinSalt",
                table: "Products");
        }
    }
}
