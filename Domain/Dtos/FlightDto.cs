namespace backend_test.Domain.Dtos;

public class FlightDto
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AirlineId { get; set; }
}