using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]

    public class AdminApiController : ControllerBase
    {
        private readonly FlightStorage _storage;
        private static readonly object lockObject = new object();

        public AdminApiController()
        {
            _storage = new FlightStorage();
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            return NotFound();
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            lock (lockObject)
            {
                var flight = _storage.GetFlight(id);
                if (flight == null)
                {
                    return Ok();
                }

                _storage.DeleteFlight(id);
                return Ok();
            }
        }

        [Route("flights")]
        [HttpPut]
        public IActionResult PutFlight(Flight flight)
        {
            lock (lockObject)
            {
                if (IsInvalidFlight(flight))
                {
                    return BadRequest();
                }

                if (!IsArrivalTimeValid(flight.DepartureTime, flight.ArrivalTime))
                {
                    return BadRequest("Arrival time must be at least 10 minutes after departure time and not exceed 2 days.");
                }

                if (flight.From.AirportCode.Equals(flight.To.AirportCode, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("From and To airports cannot be the same.");
                }

                if (FlightStorage.FlightExists(flight))
                {
                    return Conflict();
                }

                _storage.AddFlight(flight);

                return Created("", flight);
            }
        }

        private static bool IsInvalidFlight(Flight flight)
        {

            return flight == null
                || string.IsNullOrEmpty(flight.From.AirportCode)
                || string.IsNullOrEmpty(flight.To.AirportCode)
                || string.IsNullOrEmpty(flight.Carrier)
                || string.IsNullOrWhiteSpace(flight.DepartureTime)
                || string.IsNullOrWhiteSpace(flight.ArrivalTime);
        }

        private static bool IsValidAlphaNumeric(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(char.IsLetterOrDigit) && !value.Contains('\\');
        }

        private static bool IsArrivalTimeValid(string departureTime, string arrivalTime)
        {
            DateTime departure = DateTime.Parse(departureTime);
            DateTime arrival = DateTime.Parse(arrivalTime);

            TimeSpan timeDifference = arrival - departure;

            if (timeDifference.TotalMinutes < 1)
            {
                return false;
            }

            if (timeDifference.TotalDays > 10)
            {
                return false;
            }

            return true;
        }
    }
}


