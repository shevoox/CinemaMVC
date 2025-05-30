using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaMVC.Migrations
{
    /// <inheritdoc />
    public partial class Remove_TheaterId1_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Theaters_TheaterId1",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TheaterId1",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "TheaterId1",
                table: "Seats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TheaterId1",
                table: "Seats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TheaterId1",
                table: "Seats",
                column: "TheaterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Theaters_TheaterId1",
                table: "Seats",
                column: "TheaterId1",
                principalTable: "Theaters",
                principalColumn: "Id");
        }
    }
}
