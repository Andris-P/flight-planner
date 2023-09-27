using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlanner.Controllers
{
    [AllowAnonymous]
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

        //public IActionResult SearchAirports(string search)
        //{
        //    if (string.IsNullOrEmpty(search))
        //        return BadRequest("Search term is missing or empty");

        //    var airports = _storage.SearchAirportsPhrase(search);

        //    if (airports.Count == 0)
        //    {
        //        var pageResult = new PageResult<Airport>
        //        {
        //            Page = 0,
        //            TotalItems = 0,
        //            Items = new List<Airport>()
        //        };

        //        return Ok(pageResult);
        //    }

        //    return Ok(airports);
        //}
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
        public IActionResult SearchFlights(string from, string to, DateTime? departureDate,[FromBody] SearchFlightsRequest request)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || departureDate == null)
            {
                return BadRequest("Invalid request: Missing required fields.");
            }

            if (request == null || string.IsNullOrEmpty(request.From.AirportCode)
                || string.IsNullOrEmpty(request.To.AirportCode)
                || request.DepartureTime == default)
            {
                return Ok(new PageResult<Flight>
                {
                    Page = 0,
                    TotalItems = 0,
                    Items = new List<Flight>()
                });
            }return Ok();
        }
    }
}