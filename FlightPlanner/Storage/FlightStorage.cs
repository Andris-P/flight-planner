using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private static readonly object _lockObject = new object();
        //private static List<Flight> _flightStorage = new List<Flight>();
        private readonly FlightPlannerDbContext _context;
        //private static int _id = 0;

        public FlightStorage(FlightPlannerDbContext context)
        {
            _context = context;
        }

        // public FlightStorage()
        // {
        // }

        public void AddFlight(Flight flight)
        {
            lock (_lockObject)
            {
                _context.Flights.Add(flight);
                _context.SaveChanges();
            }
        }

        public void Clear()
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.SaveChanges();
            //_flightStorage.Clear();
        }

        public bool FlightExists(Flight flight)
        {
            return _context.Flights.Any(f =>
          f.From.AirportCode == flight.From.AirportCode &&
          f.To.AirportCode == flight.To.AirportCode &&
          f.Carrier == flight.Carrier &&
          f.DepartureTime == flight.DepartureTime &&
          f.ArrivalTime == flight.ArrivalTime);
        }

        //{
        //    foreach (var existingFlight in _flightStorage)
        //    {
        //        if (AreFlightsEqual(existingFlight, flight))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private static bool AreFlightsEqual(Flight flight1, Flight flight2)
        {
            return flight1.From.AirportCode == flight2.From.AirportCode
                && flight1.To.AirportCode == flight2.To.AirportCode
                && flight1.Carrier == flight2.Carrier
                && flight1.DepartureTime == flight2.DepartureTime
                && flight1.ArrivalTime == flight2.ArrivalTime;
        }

        public Flight GetFlight(int id)
        {
            return _context.Flights.FirstOrDefault(flight => flight.Id == id);
        }

        public void DeleteFlight(int id)
        {
            lock (_lockObject)
            {
                var flightToRemove = GetFlight(id);
                if (flightToRemove != null)
                {
                    _context.Flights.Remove(flightToRemove);
                    _context.SaveChanges();
                }
            }
        }

        public List<Airport> SearchAirportsPhrase(string search)
        {
            var airports = _context.Flights
                .SelectMany(flight => new[] { flight.From, flight.To })
                .Where(airport =>
                    airport.Country.ToLower().Contains(search.ToLower().Trim()) ||
                    airport.City.ToLower().Contains(search.ToLower().Trim()) ||
                    airport.AirportCode.ToLower().Contains(search.ToLower().Trim())
                ).ToList();

            return airports;
        }

        public List<Flight> SearchFlights(SearchFlightsRequest request)
        {
            return _context.Flights
                .Where(f => f.From.AirportCode == request.From &&
                            f.To.AirportCode == request.To &&
                            f.DepartureTime.Contains(request.DepartureDate))
                .ToList();
        }
    }
}
