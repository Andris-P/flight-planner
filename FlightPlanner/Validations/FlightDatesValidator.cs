using FlightPlanner.Core.Interfaces;
using FlightPlanner.Core.Models;
namespace FlightPlanner.Validations
{
    public class FlightDatesValidator : IValidate
    {
        public bool IsValid(Flight flight)
        {
            if (DateTime.TryParse(flight?.ArrivalTime, out var arrivalTime) &&
                DateTime.TryParse(flight?.DepartureTime, out var departureTime))
            {
                return arrivalTime > departureTime;
            }
            return false;
        }
    }
}
