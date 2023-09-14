namespace backend_test.Models
{
    public class Route
    {
        public int Id { get; set; }
        public int OriginCityId { get; set; }
        public int DestinationCityId { get; set; }
        public DateTime DepartureDate { get; set; }

        public ICollection<Flight> Flights { set; get; }
    }
}