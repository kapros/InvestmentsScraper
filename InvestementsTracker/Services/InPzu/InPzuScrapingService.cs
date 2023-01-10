using InPzuScraper;
using InvestementsTracker.DTO;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.InPzuScraper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using System.Globalization;
using static InvestementsTracker.Controllers.InPzuController;
using static InvestementsTracker.Helpers;

namespace InvestementsTracker.Services.InPzu
{
    public class InPzuScrapingService : IInPzuScrapingService
    {
        private readonly IConfiguration _config;
        private readonly DateFormatter _dateFormatter;
        private readonly InPzuDataContext _dataContext;

        public InPzuScrapingService(IConfiguration config, DateFormatter dateFormatter, InPzuDataContext dataContext)
        {
            _config = config;
            _dateFormatter = dateFormatter;
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<InPzuPortfolio>> ScrapePortfolios(IPage page)
        {
            var latestOrders =
                (await
                (await Login(page))
                .GoToOrders())
                .History!
                .Entries;
            var inPzuPortfolios = new List<InPzuPortfolio>();
            foreach (var order in latestOrders.Where(x => x.nazwa.Trim().Equals("KUPNO (nabycie)", StringComparison.OrdinalIgnoreCase)))
            {
                inPzuPortfolios.Add(new InPzuPortfolio
                {
                    Id = long.Parse(order.rejestrId),
                    Name = order.rejestrId,
                    Since = DateOnly.Parse(order.dataRealizacji, _dateFormatter),
                });
            }
            var existingPortfolios = await _dataContext.InPzuPortfolios.Where(x => inPzuPortfolios.Select(y => y.Id).Any(y => y == x.Id)).ToListAsync();
            await _dataContext.InPzuPortfolios.AddRangeAsync(inPzuPortfolios.Except(existingPortfolios));
            await _dataContext.SaveChangesAsync();
            return inPzuPortfolios;
        }


        public async Task<IEnumerable<InPzuOrder>> GetOrders(IPage page)
        {
            var latestOrders =
                (await
                (await Login(page))
                .GoToOrders())
                .History!
                .Entries;
            var inPzuOrders = new List<InPzuOrder>();
            foreach (var order in latestOrders.Where(x => x.nazwa.Trim().Contains("(nabycie)", StringComparison.OrdinalIgnoreCase)))
            {
                var pos = new InPzuOrder
                {
                    Id = order.zlecId,
                    OrderType = OrderType.Purchase,
                    PurchasedOn = DateOnly.Parse(order.dataRealizacji, _dateFormatter),
                    OrderName = order.zlecId.ToString(),
                    PurchaseWorth = order.wartosc,
                    Currency = order.waluta,
                    PortfolioId = long.Parse(order.rejestrId),
                    Positions = order.trn.Select(x =>
                    new InPzuPosition()
                    {
                        Id = x.trnId,
                        Units = x.liczbaJU,
                        PriceOfUnit = x.cena,
                        PurchaseValue = x.wartoscJU,
                        FundId = x.funduszId,
                        RegistrationId = x.rejestrId,
                        FundName = x.nazwa.Trim(),
                        Currency = order.waluta,
                        PurchaseDate = DateOnly.Parse(x.dataProc, _dateFormatter),
                        UnitType = x.typJU,
                        PortfolioId = long.Parse(order.rejestrId)
                    }
                    ).ToList()
                };
                inPzuOrders.Add(pos);
            }
            var existingPositions = await _dataContext.InPzuOrders.Where(x => inPzuOrders.Select(y => y.Id).Any(y => y == x.Id)).ToListAsync();
            await _dataContext.InPzuOrders.AddRangeAsync(inPzuOrders.Except(existingPositions));
            await _dataContext.SaveChangesAsync();
            return inPzuOrders;
        }

        public async Task<object> GetResultsForDate(DateOnly date, IPage page)
        {
            var pricesPage = new InPzuPricesPage(page);
            await pricesPage.GoTo();
            var pricings = await pricesPage.GetPricings(date);
            var list = new List<ReturnsDTO>();
            var results = new List<InPzuResult>();
            var pricingDate = DateOnly.ParseExact(pricings.First().FormattedDate, "yyyy-MM-dd");
            var orders = await _dataContext.InPzuOrders.Include(x => x.Positions).AsNoTracking().ToListAsync();
            foreach (var order in orders)
            {
                foreach (var (position, i) in order.Positions.OrderBy(x => x.FundId).Select((x, i) => (x, i)))
                {
                    var currentPricingForFund = pricings.First(x => x.FundName.StartsWith(position.FundName, StringComparison.OrdinalIgnoreCase) && x.UnitType == position.UnitType);
                    var fundValue = currentPricingForFund.Value;
                    var priceChange = (position.Units * fundValue) - position.PurchaseValue;
                    var percentileChange = priceChange / position.PurchaseValue * 100;
                    var result = new InPzuResult
                    {
                        Id = long.Parse(pricingDate.ToString("yyyyMMdd") + order.Id + i),
                        Date = pricingDate,
                        PositionId = position.Id,
                        Percentile = Math.Round(percentileChange, 2),
                        Value = Math.Round(priceChange, 2),
                    };
                    position.Results.Add(result);
                    results.Add(result);
                }
            }
            var minDate = results.Min(y => y.Date);
            var existingResults = await _dataContext.InPzuResults.Where(x => minDate >= x.Date).ToListAsync();
            await _dataContext.InPzuResults.AddRangeAsync(results.Except(existingResults));
            await _dataContext.SaveChangesAsync();

            var ret = orders
                .SelectMany(x => x.Positions)
                .Select(x => new
                {
                    Register = x.FundName,
                    Purchase = x.PurchaseValue.ToString("n") + Space + x.Currency,
                    PurchasedOn = x.PurchaseDate,
                    Result = x.Results.Select(r => new
                    {
                        r.Value,
                        r.Percentile
                    }).First(),
                })
                .GroupBy(x => x.Register)
                .Select(x => new { Register = x.Key, Positions = x.Select(p => new { p.Purchase, p.PurchasedOn, p.Result }) });
            return new { PricingDate = pricingDate, Results = ret };
        }

        public async Task<AccountResults> ScrapeAccount(IPage page)
        {
            await Login(page);
            var results = await (new InPzuProductsPage(page)).GoTo();
            return results;
        }

        private Task<InPzuMainPage> Login(IPage page)
        {
            var credentials = _config.GetSection("Credentials:inpzu");
            var login = credentials["login"];
            var password = credentials["password"];
            return new InPzuLogin(page)
                .Login(login, password);
        }
    }
}
