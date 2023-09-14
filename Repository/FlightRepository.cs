using backend_test.Data;
using backend_test.Domain.Dtos;
using backend_test.Models;

namespace backend_test.Repository
{
    public class FlightRepository : GenericRepository<Flight, int>, IFlightRepository
    {
        public FlightRepository(AirlineDbContext context) : base(context)
        {
        }

        public IQueryable<FlightDto> GetOne(int id)
        {
            return _context.Flights.Where(o => o.Id == id).Select(e => new FlightDto() {Id = e.Id});
        }
    }
}