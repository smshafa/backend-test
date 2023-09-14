using backend_test.Data;
using backend_test.Domain.Dtos;
using backend_test.Models;

namespace backend_test.Repository
{
    public class SubscriptionRepository : GenericRepository<Subscription, int>, ISubscriptionRepository
    {
        public SubscriptionRepository(AirlineDbContext context) : base(context)
        {
        }

        public IQueryable<SubscriptionDto> GetOne(int id)
        {
            return _context.Subscriptions.Where(o => o.Id == id).Select(e => new SubscriptionDto() {Id = e.Id});
        }
    }
}