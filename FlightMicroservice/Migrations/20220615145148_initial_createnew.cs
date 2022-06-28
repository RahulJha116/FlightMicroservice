using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightMicroservice.Migrations
{
    public partial class initial_createnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "airlineId",
                table: "Flights",
                nullable: true);

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
