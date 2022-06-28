using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightMicroservice.Migrations
{
    public partial class initial_create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    adminId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    adminName = table.Column<string>(nullable: true),
                    adminEmailId = table.Column<string>(nullable: true),
                    adminPasskey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.adminId);
                });

            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    airlineId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    airlineName = table.Column<string>(nullable: true),
                    airlineContactNumber = table.Column<int>(nullable: false),
                    airlineAddress = table.Column<string>(nullable: true),
                    airlineLogo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.airlineId);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DiscountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscountCode = table.Column<string>(nullable: true),
                    DiscountAmount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.DiscountId);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromPlace = table.Column<string>(nullable: true),
                    ToPlace = table.Column<string>(nullable: true),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false),
                    FlightNumber = table.Column<string>(nullable: true),
                    ScheduleDayOfWeek = table.Column<string>(nullable: true),
                    NoOfBusinessClassSeat = table.Column<int>(nullable: false),
                    NoOfNonBusinessClassSeat = table.Column<int>(nullable: false),
                    FlightTicketPrice = table.Column<int>(nullable: false),
                    Indicator = table.Column<int>(nullable: false),
                    airlineId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_airlineId",
                        column: x => x.airlineId,
                        principalTable: "Airlines",
                        principalColumn: "airlineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "adminId", "adminEmailId", "adminName", "adminPasskey" },
                values: new object[] { 1, "Admin1", "Admin1", "Admin1" });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "adminId", "adminEmailId", "adminName", "adminPasskey" },
                values: new object[] { 2, "Admin2", "Admin2", "Admin2" });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_airlineId",
                table: "Flights",
                column: "airlineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Airlines");
        }
    }
}
