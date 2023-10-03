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
            //lock (lockObject)
            {
                return flight == null
                    || !IsAlphaCharactersOnly(flight.From.AirportCode)
                    || !IsAlphaCharactersOnly(flight.To.AirportCode)
                    || !IsAlphaCharactersOnly(flight.Carrier)
                    || string.IsNullOrWhiteSpace(flight.DepartureTime)
                    || string.IsNullOrWhiteSpace(flight.ArrivalTime);
            }
        }

        private static bool IsAlphaCharactersOnly(string value)
        {
            //lock (lockObject) 
            {
                return !string.IsNullOrEmpty(value) && value.All(char.IsLetter) && !value.Contains('\\');
            }
        }

        private static bool IsArrivalTimeValid(string departureTime, string arrivalTime)
        {
            //lock (lockObject)
            {
                DateTime departure = DateTime.Parse(departureTime);
                DateTime arrival = DateTime.Parse(arrivalTime);

                TimeSpan timeDifference = arrival - departure;

                if (timeDifference.TotalMinutes < 10)
                {
                    return false;
                }

                if (timeDifference.TotalDays > 2)
                {
                    return false;
                }

                return true;
            }
        }
    }
}


