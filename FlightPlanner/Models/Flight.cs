using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Models
{
    public class Flight
    {
        public int Id { get; set; }

        public Airport From { get; set; }
        public Airport To { get; set; }

        [StringLength(150)]
        public string Carrier { get; set; }
        [StringLength(150)]
        public string DepartureTime { get; set; }
        [StringLength(150)]
        public string ArrivalTime { get; set; }
    }
}


