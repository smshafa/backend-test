using backend_test.Data;
using backend_test.Domain.Dtos;
using backend_test.Models;

namespace backend_test.Repository
{
    public class RouteRepository : GenericRepository<Route, int>, IRouteRepository
    {
        public RouteRepository(AirlineDbContext context) : base(context)
        {
        }

        public IQueryable<RouteDto> GetOne(int id)
        {
            var q = _context.Routes.Where(o => o.Id == id).Select(e => new RouteDto {Id = e.Id});
            return q;
        }

        public IEnumerable<Route> GetRoutesByDepartureDate(DateTime departureDate)
        {
            return _context.Routes.Where(x => x.DepartureDate == departureDate);
        }
    }
}