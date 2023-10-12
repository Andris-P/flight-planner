using FlightPlanner.Models;
//using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerAPIController : ControllerBase
    {
        //private readonly FlightStorage _storage;
        private readonly FlightPlannerDbContext _context;


        public CustomerAPIController(FlightPlannerDbContext context)
        {
           // _storage = new FlightStorage(context);
            _context = context;
        }

        [Route("airports")]
        [HttpGet]

        public IActionResult SearchAirports(string search)
        {
            if (string.IsNullOrEmpty(search))
                return BadRequest("Search term is missing or empty");

            var airports = _context.Airports
                .Where(airport =>
                    airport.Country.ToLower().Contains(search.ToLower().Trim()) ||
                    airport.City.ToLower().Contains(search.ToLower().Trim()) ||
                    airport.AirportCode.ToLower().Contains(search.ToLower().Trim())
                )
                .ToList();

            if (airports == null || airports.Count == 0)
                return NotFound("No airports found");

            return Ok(airports);
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult FindFlightById(int id)
        {
            var flight = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .SingleOrDefault(f => f.Id == id);

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

            var flights = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .Where(f => f.From.AirportCode == request.From && f.To.AirportCode == request.To && f.DepartureTime.Contains(request.DepartureDate))
                .ToList();

            return Ok(new { page = 0, totalItems = flights?.Count ?? 0, items = flights});
        }
    }
}