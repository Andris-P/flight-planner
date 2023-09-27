using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private static List<Flight> _flightStorage = new List<Flight>();
        private static int _id = 0;

        public void AddFlight(Flight flight)
        {
            flight.Id = _id++;
            _flightStorage.Add(flight);
        }

        public void Clear()
        {
            _flightStorage.Clear();
        }

        public static bool FlightExists(Flight flight)
        {
            foreach (var existingFlight in _flightStorage)
            {
                if (AreFlightsEqual(existingFlight, flight))
                {
                    return true;
                }
            }
            return false;
        }

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
            return _flightStorage.FirstOrDefault(flight => flight.Id == id);
        }

        public void DeleteFlight(int id)
        {
            var flightToRemove = GetFlight(id);
            if (flightToRemove != null)
            {
                _flightStorage.Remove(flightToRemove);
            }
        }

        public List<Airport> SearchAirportsPhrase(string search)
        {
            var airports = _flightStorage.SelectMany(flight => new[] { flight.From, flight.To })
                .Where(airport =>
                    airport.Country.ToLower().Contains(search.ToLower().Trim()) ||
                    airport.City.ToLower().Contains(search.ToLower().Trim()) ||
                    airport.AirportCode.ToLower().Contains(search.ToLower().Trim())
                ).ToList();

            return airports;
        }


    }
}
