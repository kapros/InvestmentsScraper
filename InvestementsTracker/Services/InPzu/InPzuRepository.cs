using InvestementsTracker.InPzuDatabase;
using Microsoft.EntityFrameworkCore;

namespace InvestementsTracker.Services.InPzu;
public class InPzuRepository : IInPzuRepository
{
    private readonly InPzuDataContext _dataContext;

    public InPzuRepository(InPzuDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<InPzuPortfolio>> GetPortfolios()
    {
        return await _dataContext.InPzuPortfolios.Include(x => x.Positions).ThenInclude(x => x.Results).ToListAsync();
    }
}
