namespace backend_test.Models;

public class Flight
{
    public int Id { get; set; }
    //public int RouteId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AirlineId { get; set; }
    public int RouteId { get; set; }
    public Route route { get; set; }
}