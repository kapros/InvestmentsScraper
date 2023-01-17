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

    public async Task<bool> RenamePortfolio(long id, string name)
    {
        var portfolio = _dataContext.InPzuPortfolios.Find(id);
        if (portfolio is not null)
        {
            portfolio.Name = name;
            await _dataContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
