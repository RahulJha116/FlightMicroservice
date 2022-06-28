using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightMicroservice.Migrations
{
    public partial class flight_tableMeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Meal",
                table: "Flights",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meal",
                table: "Flights");
        }
    }
}
