namespace FlightPlanner.Models
{
    public class SearchFlightsRequest
    {
        internal object DepartureTime;

        public Airport From { get; set; }
        public Airport To { get; set; }
        public DateTime Date { get; set; }
    }
}
