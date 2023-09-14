using backend_test.Domain.Dtos;
using backend_test.Models;

namespace backend_test.Repository
{
    public interface IFlightRepository : IGenericRepository<Flight, int>
    {
        IQueryable<FlightDto> GetOne(int id);
    }
}