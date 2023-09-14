namespace backend_test.Domain.Dtos;

public class SubscriptionDto
{
    public int Id { get; set; }
    public int AgencyId { get; set; }
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }
}