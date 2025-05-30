using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaMVC.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMovieIdFromBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Bookings",
                type: "int",
                nullable: true);
        }
    }
}
