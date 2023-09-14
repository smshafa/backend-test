namespace backend_test.Domain.Dtos;

public class ResultDto
{
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AirlineId { get; set; }
    public Status Status { get; set; }
}

public enum Status
{
    New,
    Discontinued
}