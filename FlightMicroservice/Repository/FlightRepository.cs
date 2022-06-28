
using FlightMicroservice.DbContextFlight;
using FlightMicroservice.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMicroservice.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightContext _dbContext;

        public FlightRepository(FlightContext flightContext)
        {
            _dbContext = flightContext;
        }
        public void AddFlights(Model.Flights flights)
        {
            #region
            //if (CheckAirlineExist(flights.AirlineId) == 0)
            //{
            //    return "airline id not exist";
            //}
            //else
            //{
            //    c = flights.AirlineId;
            //}
            #endregion

            var a = RandomString(4);
            // var b =  a+ flights.FromPlace.Substring(0,2) + flights.ToPlace.Substring(0,2);

            Flights flight = new Flights
            {
                FlightNumber = flights.FlightNumber,
                FlightBusinessClassTicketPrice = flights.FlightBusinessClassTicketPrice,
                FlightNonBusinessClassTicketPrice = flights.FlightNonBusinessClassTicketPrice,
                FromPlace = flights.FromPlace,
                ToPlace = flights.ToPlace,
                ScheduleDayOfWeek = flights.ScheduleDayOfWeek,
                StartDateTime = flights.StartDateTime ,
                EndDateTime = flights.EndDateTime,
                NoOfBusinessClassSeat = flights.NoOfBusinessClassSeat,
                NoOfNonBusinessClassSeat = flights.NoOfNonBusinessClassSeat,
                LeftBuisnessClassSeat = flights.NoOfBusinessClassSeat,
                LeftNonBuisnessClassSeat = flights.NoOfNonBusinessClassSeat,
                Meal = flights.Meal,
                airlineId = flights.airlineId
            };

            _dbContext.Add(flight);
            Save();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public void UnBlockFlight(int airlineId)
        {


            foreach (var value in _dbContext.Flights)
            {
                if (value.airlineId == airlineId)
                {
                    value.Indicator = 0;

                }


            }
            Save();




        }

        //public int CheckAirlineExist(int airlineId)
        //{
        //    if (_dbContext.Airlines.Find(airlineId) != null)
        //        return airlineId;
        //    return 0;

        //}

        public void DeleteFlight(int FlightId)
        {
            var f = _dbContext.Flights.Find(FlightId);
            _dbContext.Flights.Remove(f);
            Save();
        }

        public Flights GetFlightByID(int flightId)
        {
            return _dbContext.Flights.Find(flightId);
        }

        public IEnumerable<Model.Flights> GetFlights()
        {
            return _dbContext.Flights.ToList();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateFlight(Model.Flights flight)
        {
            _dbContext.Entry(flight).State = EntityState.Modified;
            Save();
        }

        public void BlockFlight(int airlineId)
        {

            foreach (var value in _dbContext.Flights)
            {
                if (value.airlineId == airlineId)
                {
                    value.Indicator = 1;

                }


            }
            Save();
        }

        public List<Flights> GetFlightByAirlineId(int airlineId)
        {
            List<Flights> f = new List<Flights>();

            foreach (var value in _dbContext.Flights)
            {
                if (value.airlineId == airlineId)
                {
                    f.Add(value);

                }


            }
            return f;

        }
    }
}
