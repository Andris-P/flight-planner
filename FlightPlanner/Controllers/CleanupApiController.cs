 using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class CleanupApiController : ControllerBase
    {
        private readonly FlightStorage _storage;
        private readonly FlightPlannerDbContext _context;


        public CleanupApiController(FlightPlannerDbContext context)
        {
            _storage = new FlightStorage(context);
            _context = context;
        }

        [Route("clear")]
        [HttpPost]
        public IActionResult Clear()
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.Airports.RemoveRange(_context.Airports);
            _context.SaveChanges();
            //_storage.Clear();

            return Ok();
        }
    }
}

 