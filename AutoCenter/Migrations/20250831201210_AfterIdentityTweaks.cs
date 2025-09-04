using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCenter.Web.Migrations
{
    /// <inheritdoc />
    public partial class AfterIdentityTweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Listings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Listings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "Id",
                keyValue: 2,
                column: "OwnerID",
                value: null);
        }
    }
}
