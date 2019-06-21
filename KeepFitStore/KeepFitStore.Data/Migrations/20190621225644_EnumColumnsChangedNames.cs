using Microsoft.EntityFrameworkCore.Migrations;

namespace KeepFitStore.Data.Migrations
{
    public partial class EnumColumnsChangedNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vitamin_Type",
                table: "Products",
                newName: "VitaminType");

            migrationBuilder.RenameColumn(
                name: "Protein_Type",
                table: "Products",
                newName: "ProteinType");

            migrationBuilder.RenameColumn(
                name: "ProteinPerServing",
                table: "Products",
                newName: "ProteinProteinPerServing");

            migrationBuilder.RenameColumn(
                name: "Protein_Fat",
                table: "Products",
                newName: "ProteinFat");

            migrationBuilder.RenameColumn(
                name: "Protein_EnergyPerServing",
                table: "Products",
                newName: "ProteinEnergyPerServing");

            migrationBuilder.RenameColumn(
                name: "Protein_Carbohydrate",
                table: "Products",
                newName: "ProteinCarbohydrate");

            migrationBuilder.RenameColumn(
                name: "Creatine_Type",
                table: "Products",
                newName: "CreatineType");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Products",
                newName: "AminoAcidType");

            migrationBuilder.RenameColumn(
                name: "Fat",
                table: "Products",
                newName: "AminoAcidFat");

            migrationBuilder.RenameColumn(
                name: "EnergyPerServing",
                table: "Products",
                newName: "AminoAcidEnergyPerServing");

            migrationBuilder.RenameColumn(
                name: "Carbohydrate",
                table: "Products",
                newName: "AminoAcidCarbohydrate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VitaminType",
                table: "Products",
                newName: "Vitamin_Type");

            migrationBuilder.RenameColumn(
                name: "ProteinType",
                table: "Products",
                newName: "Protein_Type");

            migrationBuilder.RenameColumn(
                name: "ProteinProteinPerServing",
                table: "Products",
                newName: "ProteinPerServing");

            migrationBuilder.RenameColumn(
                name: "ProteinFat",
                table: "Products",
                newName: "Protein_Fat");

            migrationBuilder.RenameColumn(
                name: "ProteinEnergyPerServing",
                table: "Products",
                newName: "Protein_EnergyPerServing");

            migrationBuilder.RenameColumn(
                name: "ProteinCarbohydrate",
                table: "Products",
                newName: "Protein_Carbohydrate");

            migrationBuilder.RenameColumn(
                name: "CreatineType",
                table: "Products",
                newName: "Creatine_Type");

            migrationBuilder.RenameColumn(
                name: "AminoAcidType",
                table: "Products",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "AminoAcidFat",
                table: "Products",
                newName: "Fat");

            migrationBuilder.RenameColumn(
                name: "AminoAcidEnergyPerServing",
                table: "Products",
                newName: "EnergyPerServing");

            migrationBuilder.RenameColumn(
                name: "AminoAcidCarbohydrate",
                table: "Products",
                newName: "Carbohydrate");
        }
    }
}
