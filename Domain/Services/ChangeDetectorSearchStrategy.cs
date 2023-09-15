using backend_test.Domain.Dtos;
using backend_test.Repository;

namespace backend_test.Domain.Services;

public class ChangeDetectorSearchStrategy : ChangeDetectorStrategy
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeDetectorSearchStrategy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Calculate(DateOnly startDate, DateOnly endDate, int agencyId)
    {
        DateTime start = DateTime.Now;
        var startDateAsDateTime = startDate.ToDateTime(new TimeOnly(0, 0, 0));
        var endDateAsDateTime = endDate.ToDateTime(new TimeOnly(0, 0, 0));
        var agencyLimitation = _unitOfWork.Subscriptions.All.Where(x => x.AgencyId == agencyId);

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

        // TODO: try to Use DbFunction to avoid using ToList
        var desiredFlightSetTwo = from currentFlight in flightsWithExtra.ToList()
            let isNew = !flightsWithExtra.Any(g =>
                (currentFlight.DepartureTime >= startDate.ToDateTime(new TimeOnly(0, 0, 0)) &&
                 currentFlight.DepartureTime <= endDate.ToDateTime(new TimeOnly(0, 0, 0))) &&
                currentFlight.AirlineId == g.AirlineId &&
                currentFlight.DepartureTime >= g.DepartureTime.AddDays(-7).AddMinutes(-30) &&
                g.DepartureTime <= currentFlight.DepartureTime)
            let isDiscontinued = !flightsWithExtra.Any(g =>
                (currentFlight.DepartureTime >= startDate.ToDateTime(new TimeOnly(0, 0, 0)) &&
                 currentFlight.DepartureTime <= endDate.ToDateTime(new TimeOnly(0, 0, 0))) &&
                currentFlight.AirlineId == g.AirlineId &&
                currentFlight.DepartureTime >=
                g.DepartureTime.AddDays(7).AddMinutes(-30) &&
                g.DepartureTime <= currentFlight.DepartureTime)
            select new ResultDto()
            {
                AirlineId = currentFlight.AirlineId,
                OriginCityId = currentFlight.OriginCityId,
                DepartureTime = currentFlight.DepartureTime,
                DestinationCityId = currentFlight.DestinationCityId,
                ArrivalTime = currentFlight.ArrivalTime,
                Status = isDiscontinued ? Status.Discontinued : Status.New
            };

        var resultSetTwo = desiredFlightSetTwo
            .Where(r => r.DepartureTime >= startDateAsDateTime && r.DepartureTime <= endDateAsDateTime).ToList();

        var end = DateTime.Now;
        var elapsed = end - start;
        Console.WriteLine("Elapsed time: {0}, count: {1}", elapsed, resultSetTwo.Count());

        Console.WriteLine("Creating CSV file...");
        IWriter<ResultDto> csvWriter = new CsvHelper<ResultDto>();
        csvWriter.WriteToFile(resultSetTwo, Environment.CurrentDirectory, "result");
        Console.WriteLine("CSV file is created!");
    }
}