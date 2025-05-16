using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaMVC.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAvailableSeatsCommingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableSeats",
                table: "Showtimes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableSeats",
                table: "Showtimes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
