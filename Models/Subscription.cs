namespace backend_test.Models;

public class Subscription
{
    public int Id { get; set; }
    public int AgencyId { get; set; }
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }
}