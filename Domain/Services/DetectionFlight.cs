using backend_test.Data;
using backend_test.Repository;

namespace backend_test.Domain.Services;

public class DetectionFlight : IDetectionFlight
{
    //private readonly IRouteRepository _routeRepository;
    //private readonly IFlightRepository _flightRepository;
    //private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    //public DetectionFlight(IUnitOfWork unitOfWork,IRouteRepository routeRepository, IFlightRepository flightRepository,
    //    ISubscriptionRepository subscriptionRepository)
    //{
    //    _unitOfWork = unitOfWork;
    //    _routeRepository = routeRepository;
    //    _flightRepository = flightRepository;
    //    _subscriptionRepository = subscriptionRepository;
    //}
    //public DetectionFlight(IRouteRepository routeRepository, IFlightRepository flightRepository, ISubscriptionRepository subscriptionRepository)
    //{
    //    _routeRepository = routeRepository;
    //    _flightRepository = flightRepository;
    //    _subscriptionRepository = subscriptionRepository;
    //}


    public DetectionFlight(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void DetectFlight(DateOnly startDate, DateOnly endDate, int agencyId)
    {
        var agencyLimitation = _unitOfWork.Subscriptions.All.Where(x => x.AgencyId == agencyId);
        //var agencyLimitation = _subscriptionRepository.All.Where(x => x.AgencyId == agencyId);

        var t1 = agencyLimitation.ToList();
    }
}