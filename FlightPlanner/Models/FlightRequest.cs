using FlightPlanner.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Models
{
    public class FlightRequest
    {
        public int Id { get; set; }
        public AirportRequest From { get; set; }
        public AirportRequest To { get; set; }

        [StringLength(150)]
        public string Carrier { get; set; }
        [StringLength(150)]
        public string DepartureTime { get; set; }
        [StringLength(150)]
        public string ArrivalTime { get; set; }
    }
}
