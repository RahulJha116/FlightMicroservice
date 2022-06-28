using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightMicroservice.Migrations
{
    public partial class flight_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlightTicketPrice",
                table: "Flights",
                newName: "LeftNonBuisnessClassSeat");

            migrationBuilder.AddColumn<int>(
                name: "FlightBusinessClassTicketPrice",
                table: "Flights",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlightNonBusinessClassTicketPrice",
                table: "Flights",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeftBuisnessClassSeat",
                table: "Flights",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlightBusinessClassTicketPrice",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightNonBusinessClassTicketPrice",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "LeftBuisnessClassSeat",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "LeftNonBuisnessClassSeat",
                table: "Flights",
                newName: "FlightTicketPrice");
        }
    }
}
