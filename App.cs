using backend_test.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace backend_test;

public class App
{
    private readonly IConfiguration _configuration;
    private readonly IDetectionFlight _detectionFlight;

    public App(IDetectionFlight detectionFlight, IConfiguration configuration)
    {
        _detectionFlight = detectionFlight;
        _configuration = configuration;
    }

    public void Run(string[] args)
    {
        // Console.WriteLine("Please input values:");
        // Console.WriteLine("start date (in yyyy-mm-dd format):");
        // DateOnly startDate = DateOnly.ParseExact(Console.ReadLine(), "yyyy-MM-dd", null);
        // Console.WriteLine("end date (in yyyy-mm-dd format):");
        // DateOnly endDate = DateOnly.ParseExact(Console.ReadLine(), "yyyy-MM-dd", null);
        // Console.WriteLine("agency id:");
        // var agencyId = Int32.Parse(Console.ReadLine());
        DateOnly startDate = DateOnly.ParseExact("2016-11-16", "yyyy-MM-dd", null);
        DateOnly endDate = DateOnly.ParseExact("2016-11-18", "yyyy-MM-dd", null);
        var agencyId = 7;
        _detectionFlight.DetectFlight(startDate, endDate, agencyId);
    }
}