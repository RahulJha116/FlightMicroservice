using FlightMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMicroservice.Repository
{
    public interface IFlightRepository
    {
        IEnumerable<Model.Flights> GetFlights();
        Model.Flights GetFlightByID(int flightId);

        void AddFlights(Model.Flights flights);
        void DeleteFlight(int FlightId);
        void UpdateFlight(Model.Flights flight);
        void Save();

        void BlockFlight(int airlineId);
        void UnBlockFlight(int airlineId);

       List<Flights> GetFlightByAirlineId(int airlineId);
    }
}
