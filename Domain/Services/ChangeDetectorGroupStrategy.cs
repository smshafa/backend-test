using backend_test.Domain.Dtos;
using backend_test.Repository;

namespace backend_test.Domain.Services;

public class ChangeDetectorGroupStrategy : ChangeDetectorStrategy
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ChangeDetectorGroupStrategy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public override void Calculate(DateOnly startDate, DateOnly endDate, int agencyId)
    {
        DateTime start = DateTime.Now;
        var startDateAsDateTime = startDate.ToDateTime(new TimeOnly(0, 0, 0));
        var endDateAsDateTime = endDate.ToDateTime(new TimeOnly(0, 0, 0));
        var agencyLimitation = _unitOfWork.Subscriptions.All.Where(x => x.AgencyId == agencyId);
        
        // --- Add 7 days to star and end date
        DateTime decreasedStartDateTime = startDate.ToDateTime(new TimeOnly(0, 0, 0)).AddDays(-7).AddMinutes(-30);
        DateTime extraEndDateTime = endDate.ToDateTime(new TimeOnly(0, 0, 0)).AddDays(7).AddMinutes(30);

        var equalPlaces = from r in _unitOfWork.Routes.All
            join a in agencyLimitation on new {r.OriginCityId, r.DestinationCityId} equals new
                {a.OriginCityId, a.DestinationCityId}
            select r;

        var flightsWithExtra = from f in _unitOfWork.Flights.All
            join r in equalPlaces on f.RouteId equals r.Id
            where f.DepartureTime >= decreasedStartDateTime && f.DepartureTime <= extraEndDateTime
            select new
            {
                f.Id, f.AirlineId, f.RouteId, f.DepartureTime, f.ArrivalTime, r.DepartureDate, r.OriginCityId,
                r.DestinationCityId
            };
        
        var resultsGroup = from f in flightsWithExtra
            group f by new {f.RouteId, f.AirlineId}
            into g
            select new
            {
                g.Key.RouteId, g.Key.AirlineId, MaxDate = g.Max(s => s.DepartureTime),
                MinDate = g.Min(s => s.DepartureTime)
            };
        

        var desiredFlightSetOne = from r in flightsWithExtra
            let isNew = resultsGroup.Any(g =>
                // (r.DepartureTime >= startDateAsDateTime && r.DepartureTime <= endDateAsDateTime) &&
                r.AirlineId == g.AirlineId &&
                r.RouteId == g.RouteId &&
                r.DepartureTime >= g.MinDate &&
                g.MaxDate <= r.DepartureTime)
            let isDiscontinued = resultsGroup.Any(g =>
                // (r.DepartureTime >= startDateAsDateTime && r.DepartureTime <= endDateAsDateTime) &&
                r.AirlineId == g.AirlineId &&
                r.RouteId == g.RouteId &&
                r.DepartureTime >= g.MinDate &&
                g.MaxDate <= r.DepartureTime)
            select new ResultDto()
            {
                AirlineId = r.AirlineId,
                OriginCityId = r.OriginCityId,
                DepartureTime = r.DepartureTime,
                DestinationCityId = r.DestinationCityId,
                ArrivalTime = r.ArrivalTime,
                Status = isDiscontinued ? Status.Discontinued : Status.New
            };

        var resultSetOne = desiredFlightSetOne
            .Where(r => r.DepartureTime >= startDateAsDateTime && r.DepartureTime <= endDateAsDateTime).ToList();
        
        DateTime end = DateTime.Now;
        TimeSpan elapsed = end - start;
        Console.WriteLine("Elapsed time: {0}, count: {1}", elapsed, resultSetOne.Count());
        
        Console.WriteLine("Creating CSV file...");
        IWriter<ResultDto> csvWriter = new CsvHelper<ResultDto>();
        csvWriter.WriteToFile(resultSetOne, Environment.CurrentDirectory, "result");
        Console.WriteLine("CSV file is created!");
    }
}