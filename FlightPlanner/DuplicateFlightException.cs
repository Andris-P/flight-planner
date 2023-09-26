namespace FlightPlanner
{
    public class DuplicateFlightException :Exception
    {
        public DuplicateFlightException () : base ("Flight with the same attributes already exists.")
        {
        }
    }
}
