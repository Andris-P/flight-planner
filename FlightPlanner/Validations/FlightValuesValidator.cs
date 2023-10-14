using FlightPlanner.Core.Interfaces;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Validations
{
    public class FlightValuesValidator : IValidate
    {
        public bool IsValid(Flight flight)
        {
            return !string.IsNullOrEmpty(flight?.ArrivalTime) &&
                !string.IsNullOrEmpty(flight?.DepartureTime) &&
                !string.IsNullOrEmpty(flight?.Carrier) &&
                flight?.To != null &&
                flight?.From != null;
        }
    }
}
