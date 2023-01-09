using InvestementsTracker.InPzuDatabase;

namespace InvestementsTracker.Services.InPzu;

public interface IInPzuRepository
{
    Task<IEnumerable<InPzuPortfolio>> GetPortfolios();
}
