using backend_test.Repository;

namespace backend_test.Domain.Services;

public class DetectionFlight : IDetectionFlight
{
    private readonly IUnitOfWork _unitOfWork;

    public DetectionFlight(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void DetectFlight(DateOnly startDate, DateOnly endDate, int agencyId)
    {
        ChangeDetectorStrategy groupStrategy = new ChangeDetectorGroupStrategy(_unitOfWork);
        groupStrategy.Calculate(startDate, endDate, agencyId);
        
        // ChangeDetectorStrategy searchStrategy = new ChangeDetectorSearchStrategy(_unitOfWork);
        // searchStrategy.Calculate(startDate, endDate, agencyId);
    }
    
}