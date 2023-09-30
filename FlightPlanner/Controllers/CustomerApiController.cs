using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerAPIController : ControllerBase
    {
        private readonly FlightStorage _storage;

        public CustomerAPIController()
        {
            _storage = new FlightStorage();
        }

        [Route("airports")]
        [HttpGet]

        public IActionResult SearchAirports(string search)
        {
            if (string.IsNullOrEmpty(search))
                return BadRequest("Search term is missing or empty");

            var airports = _storage.SearchAirportsPhrase(search);

            if (airports == null)
                return NotFound("No airports found");

            return Ok(airports);
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult FindFlightById(int id)
        {
            var flight = _storage.GetFlight(id);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights([FromBody] SearchFlightsRequest request)
        {

            if (request.From.Equals(request.To, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("From and To airports cannot be the same.");
            }

            if (string.IsNullOrEmpty(request.From) || string.IsNullOrEmpty(request.To) || request.DepartureDate == default)
            {
                return BadRequest("Invalid request: Missing required fields.");
            }

            var flights = _storage.SearchFlights(request);
            return Ok(new { page = 0, totalItems = flights?.Count ?? 0, items = flights ?? new List<Flight>() });
        }
    }
}