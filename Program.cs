using backend_test;
using backend_test.Data;
using backend_test.Domain.Services;
using backend_test.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

IHostBuilder CreateHostBuilder(string[] strings)
{
    string connectionstring = @"Server=.;Database=AirlineDB;User Id=sa;Password=123456;TrustServerCertificate=true;";
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<App>();
            services.AddDbContext<IDbContext, AirlineDbContext>(options => options.UseSqlServer(connectionstring),
                ServiceLifetime.Singleton);
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IDetectionFlight, DetectionFlight>();
            services.AddSingleton<IRouteRepository, RouteRepository>();
            services.AddSingleton<IFlightRepository, FlightRepository>();
            services.AddSingleton<ISubscriptionRepository, SubscriptionRepository>();
        });
    // .ConfigureAppConfiguration(app => { app.AddJsonFile("appsettings.json"); });
}