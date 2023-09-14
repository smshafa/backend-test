using backend_test.Domain.Dtos;
using backend_test.Models;

namespace backend_test.Repository
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription, int>
    {
        IQueryable<SubscriptionDto> GetOne(int id);
    }
}