﻿// <auto-generated />
using System;
using FlightMicroservice.DbContextFlight;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlightMicroservice.Migrations
{
    [DbContext(typeof(FlightContext))]
    [Migration("20220615115245_initial_create")]
    partial class initial_create
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FlightMicroservice.Model.Admin", b =>
                {
                    b.Property<int>("adminId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("adminEmailId");

                    b.Property<string>("adminName");

                    b.Property<string>("adminPasskey");

                    b.HasKey("adminId");

                    b.ToTable("Admin");

                    b.HasData(
                        new { adminId = 1, adminEmailId = "Admin1", adminName = "Admin1", adminPasskey = "Admin1" },
                        new { adminId = 2, adminEmailId = "Admin2", adminName = "Admin2", adminPasskey = "Admin2" }
                    );
                });

            modelBuilder.Entity("FlightMicroservice.Model.Airline", b =>
                {
                    b.Property<int>("airlineId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("airlineAddress");

                    b.Property<int>("airlineContactNumber");

                    b.Property<string>("airlineLogo");

                    b.Property<string>("airlineName");

                    b.HasKey("airlineId");

                    b.ToTable("Airlines");
                });

            modelBuilder.Entity("FlightMicroservice.Model.Discount", b =>
                {
                    b.Property<int>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DiscountAmount");

                    b.Property<string>("DiscountCode");

                    b.HasKey("DiscountId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("FlightMicroservice.Model.Flights", b =>
                {
                    b.Property<int>("FlightId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDateTime");

                    b.Property<string>("FlightNumber");

                    b.Property<int>("FlightTicketPrice");

                    b.Property<string>("FromPlace");

                    b.Property<int>("Indicator");

                    b.Property<int>("NoOfBusinessClassSeat");

                    b.Property<int>("NoOfNonBusinessClassSeat");

                    b.Property<string>("ScheduleDayOfWeek");

                    b.Property<DateTime>("StartDateTime");

                    b.Property<string>("ToPlace");

                    b.Property<int?>("airlineId");

                    b.HasKey("FlightId");

                    b.HasIndex("airlineId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("FlightMicroservice.Model.Flights", b =>
                {
                    b.HasOne("FlightMicroservice.Model.Airline", "AirlineId")
                        .WithMany()
                        .HasForeignKey("airlineId");
                });
#pragma warning restore 612, 618
        }
    }
}
