namespace backend_test.Domain.Dtos;

public class RouteDto
{
    public int Id { get; set; }
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }
    public DateTime DepartureDate { get; set; }
}