namespace backend_test.Domain.Services;

public abstract class ChangeDetectorStrategy
{
    public abstract void Calculate(DateOnly startDate, DateOnly endDate, int agencyId);
}