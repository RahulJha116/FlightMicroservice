using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightMicroservice.Migrations
{
    public partial class airline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "airlineId",
                table: "Flights",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_airlineId",
                table: "Flights",
                column: "airlineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airlines_airlineId",
                table: "Flights",
                column: "airlineId",
                principalTable: "Airlines",
                principalColumn: "airlineId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airlines_airlineId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_airlineId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "airlineId",
                table: "Flights");
        }
    }
}
