using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCenter.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    VehicleSpecs_Make = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleSpecs_Model = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleSpecs_Year = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleSpecs_Color = table.Column<int>(type: "INTEGER", nullable: true),
                    VehicleSpecs_Mileage = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleSpecs_Vin = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleSpecs_Transmission = table.Column<int>(type: "INTEGER", nullable: true),
                    VehicleSpecs_FuelType = table.Column<int>(type: "INTEGER", nullable: true),
                    VehicleSpecs_BodyType = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Listings");
        }
    }
}
