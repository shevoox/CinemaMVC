using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddBookedNumberToMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookedNumber",
                table: "Movies",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookedNumber",
                table: "Movies");
        }
    }
}
