using backend_test.Data;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace backend_test.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;
        public IRouteRepository Routes { get; set; }
        public IFlightRepository Flights { get; set; }
        public ISubscriptionRepository Subscriptions { get; set; }

        // public UnitOfWork(IDbContext dbContext)
        // {
        //     this._context = dbContext;
        // }

        public UnitOfWork(IDbContext dbContext, IRouteRepository routeRepository, IFlightRepository flightRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _context = dbContext;
            Routes = routeRepository;
            Flights = flightRepository;
            Subscriptions = subscriptionRepository;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}