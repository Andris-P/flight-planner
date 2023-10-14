using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        public FlightService(IFlightPlannerDbContext context) : base(context) { }


        public Flight? GetFullFlightById(int id)
        {
            return _context.Flights
                .Include(f => f.To)
                .Include(f => f.From)
                .SingleOrDefault(f => f.Id == id);
        }

        public bool Exists(Flight flight)
        {
            return _context.Flights
                .Any(f => f.ArrivalTime == flight.ArrivalTime &&
                    f.DepartureTime == flight.DepartureTime &&
                    f.Carrier == flight.Carrier &&
                    f.To.AirportCode == flight.To.AirportCode &&
                    f.From.AirportCode == flight.From.AirportCode);
        }
    }
}
