using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCenter.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Listings",
                columns: new[] { "Id", "Description", "IsActive", "Price", "Title", "VehicleSpecs_BodyType", "VehicleSpecs_Color", "VehicleSpecs_FuelType", "VehicleSpecs_Make", "VehicleSpecs_Mileage", "VehicleSpecs_Model", "VehicleSpecs_Transmission", "VehicleSpecs_Vin", "VehicleSpecs_Year" },
                values: new object[] { 2, "A sleek and stylish coupe", true, 12000m, "BMW 320i Coupe", 3, 1, 0, "BMW", "150000", "320i", 1, "WBAVC31050KT12345", "2013" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Listings",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
