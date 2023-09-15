using backend_test.Data;
using backend_test.Repository;
using Microsoft.EntityFrameworkCore;

namespace backend_test.Domain.Services;

public class DetectionFlight : IDetectionFlight
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DetectionFlight(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void DetectFlight(DateOnly startDate, DateOnly endDate, int agencyId)
    {
        var agencyLimitation = _unitOfWork.Subscriptions.All.Where(x => x.AgencyId == agencyId);
        var interestingRouteForAgency = from r in _unitOfWork.Routes.All
            join a in agencyLimitation on new {r.OriginCityId, r.DestinationCityId} equals new
                {a.OriginCityId, a.DestinationCityId}
            where r.DepartureDate >= startDate.ToDateTime(new TimeOnly(0, 0, 0)) && r.DepartureDate <= endDate.ToDateTime(new TimeOnly(0, 0, 0)) 
            select r;
        
        var flightsInInterestingRoute = from f in _unitOfWork.Flights.All
            join r in interestingRouteForAgency on f.RouteId equals r.Id
            select new
            {
                f,
                r
            };
        
        
        DateTime startDateTime = startDate.ToDateTime(new TimeOnly(0, 0, 0)).AddDays(-7).AddMinutes(-30);
        DateTime endDateTime = endDate.ToDateTime(new TimeOnly(0, 0, 0)).AddDays(7).AddMinutes(30);
        
        var t = from r in _unitOfWork.Routes.All
            join a in agencyLimitation on new {r.OriginCityId, r.DestinationCityId} equals new
                {a.OriginCityId, a.DestinationCityId}
            select r;
        
        var results = from f in _unitOfWork.Flights.All
            join r in t on f.RouteId equals r.Id
            where f.DepartureTime >= startDateTime && f.DepartureTime <= endDateTime
            select f;

        var ttt = results.ToList();

        var rrr = from r in results.ToList()
            let isNew = !results.Any(g => r.AirlineId == g.AirlineId && 
                                          r.DepartureTime >= g.DepartureTime.AddDays(-7).AddMinutes(-30) && g.DepartureTime <= r.DepartureTime)
            // let isDiscontinued = !results.Any(g => r.AirlineId == g.AirlineId && 
            //                                        r.DepartureTime >= g.DepartureTime.AddDays(7).AddMinutes(-30) && g.DepartureTime <= r.DepartureTime)
            select new
            {
                results, isNew
                // Result = isDiscontinued? "Discontinued" : "New" 
            };

        var rr = rrr.ToList();
        // var t1 = agencyLimitation.ToList();
    }
}