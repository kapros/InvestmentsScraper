using InPzuScraper;
using InvestementsTracker.InPzuDatabase;
using Microsoft.Playwright;

namespace InvestementsTracker.Services.InPzu;

public interface IInPzuScrapingService
{
    Task<IEnumerable<InPzuPortfolio>> ScrapePortfolios(IPage page);
    Task<IEnumerable<InPzuOrder>> GetOrders(IPage page);
    Task<object> GetResultsForDate(DateOnly date, IPage page);
    Task<AccountResults> ScrapeAccount(IPage page);
}
