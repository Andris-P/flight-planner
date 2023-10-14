using FlightPlanner.Core.Interfaces;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Validations
{
    public class DuplicateFlightValidator : IValidate
    {
        private readonly IFlightService _flightService;

        public DuplicateFlightValidator(IFlightService flightService)
        {
            _flightService = flightService;
        }

        public bool IsValid(Flight flight)
        {
            return !_flightService.Exists(flight);
        }
    }
}
 