using backend_test.Data;
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
        var agencyLimitation = _unitOfWork.Subscriptions.All.Where(x => x.AgencyId == agencyId);

        var t1 = agencyLimitation.ToList();
    }
}