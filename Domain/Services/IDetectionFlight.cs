namespace backend_test.Domain.Services;

public interface IDetectionFlight
{
    void DetectFlight(DateOnly startDate, DateOnly endDate, int agencyId);
}