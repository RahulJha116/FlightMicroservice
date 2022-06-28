using FlightMicroservice.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMicroservice.DbContextFlight
{
    public class FlightContext: DbContext
    {
        public FlightContext()
        {
        }

        public FlightContext(DbContextOptions<FlightContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("FlightMicroserviceDB");
                optionsBuilder.UseSqlServer(connectionString);

            }

        }

        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Flights> Flights { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Admin> Admin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    adminId = 1,
                    adminName = "Admin1",
                    adminEmailId = "Admin1",
                    adminPasskey = "Admin1"
                },
                new Admin
                {
                    adminId = 2,
                    adminName = "Admin2",
                    adminEmailId = "Admin2",
                    adminPasskey = "Admin2"
                });

        }


    }
}

