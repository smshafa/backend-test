using System.Reflection;
using backend_test.Domain.Dtos;
using backend_test.Repository;

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
        DateTime start = DateTime.Now;
        var startDateAsDateTime = startDate.ToDateTime(new TimeOnly(0, 0, 0));
        var endDateAsDateTime = endDate.ToDateTime(new TimeOnly(0, 0, 0));


        var agencyLimitation = _unitOfWork.Subscriptions.All.Where(x => x.AgencyId == agencyId);
        var interestingRouteForAgency = from r in _unitOfWork.Routes.All
            join a in agencyLimitation on new {r.OriginCityId, r.DestinationCityId} equals new
                {a.OriginCityId, a.DestinationCityId}
            where r.DepartureDate >= startDateAsDateTime && r.DepartureDate <= endDateAsDateTime
            select r;

        var flightsInInterestingRoute = from f in _unitOfWork.Flights.All
            join r in interestingRouteForAgency on f.RouteId equals r.Id
            select new
            {
                f.Id, f.DepartureTime, f.RouteId, f.AirlineId, f.ArrivalTime, r.DepartureDate, r.DestinationCityId,
                r.OriginCityId
            };
        

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
        
        //---------
        
        
        // Method 1
        var resultsGroup = from f in flightsWithExtra
            group f by new {f.RouteId, f.AirlineId}
            into g
            select new
            {
                g.Key.RouteId, g.Key.AirlineId, MaxDate = g.Max(s => s.DepartureTime),
                MinDate = g.Min(s => s.DepartureTime)
            };
        

        var desiredFlightSetOne = from r in flightsWithExtra.ToList()
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
        IWriter<ResultDto> csvWriter = new CsvHelper<ResultDto>();
        csvWriter.WriteToFile(resultSetOne, Environment.CurrentDirectory, "result");

        DateTime end = DateTime.Now;
        TimeSpan elapsed = end - start;
        Console.WriteLine("Elapsed time: {0}, count: {1}", elapsed, resultSetOne.Count());

        

        // Method 2
        start = DateTime.Now;
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

        end = DateTime.Now;
        elapsed = end - start;
        Console.WriteLine("Elapsed time: {0}, count: {1}", elapsed, resultSetTwo.Count());
    }

    /// <summary>
    /// Creates the CSV from a generic list.
    /// </summary>;
    /// <typeparam name="T"></typeparam>;
    /// <param name="list">The list.</param>;
    /// <param name="csvNameWithExt">Name of CSV (w/ path) w/ file ext.</param>;
    public static void CreateCSVFromGenericList<T>(List<T> list, string csvCompletePath)
    {
        if (list == null || list.Count == 0) return;

        //get type from 0th member
        Type t = list[0].GetType();
        string newLine = Environment.NewLine;

        if (!Directory.Exists(Path.GetDirectoryName(csvCompletePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(csvCompletePath));

        if (!File.Exists(csvCompletePath)) File.Create(csvCompletePath);

        using (var sw = new StreamWriter(csvCompletePath))
        {
            //make a new instance of the class name we figured out to get its props
            object o = Activator.CreateInstance(t);
            //gets all properties
            PropertyInfo[] props = o.GetType().GetProperties();

            //foreach of the properties in class above, write out properties
            //this is the header row
            sw.Write(string.Join(",", props.Select(d => d.Name).ToArray()) + newLine);

            //this acts as datarow
            foreach (T item in list)
            {
                //this acts as datacolumn
                var row = string.Join(",", props.Select(d => item.GetType()
                        .GetProperty(d.Name)
                        .GetValue(item, null)
                        .ToString())
                    .ToArray());
                sw.Write(row + newLine);
            }
        }
    }
}