using backend_test.Data;

namespace backend_test.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRouteRepository Routes { get; }
        IFlightRepository Flights { get; }
        ISubscriptionRepository Subscriptions { get; }

        int Complete();
    }
}