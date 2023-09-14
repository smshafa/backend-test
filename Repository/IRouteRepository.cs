using backend_test.Domain.Dtos;
using backend_test.Models;

namespace backend_test.Repository
{
    public interface IRouteRepository : IGenericRepository<Route, int>
    {
        IEnumerable<Route> GetRoutesByDepartureDate(DateTime departureDate);
        IQueryable<RouteDto> GetOne(int id);
    }
}