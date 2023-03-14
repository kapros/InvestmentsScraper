using Microsoft.Playwright;
using Newtonsoft.Json;

namespace InvestementsTracker.InPzuScraper
{
    public class InPzuPricesPage
    {
        private const string URL = "https://www.pzu.pl/dla-ciebie-i-rodziny/inwestycje-i-oszczednosci/wykresy/tfi-zbiorcze";
        private readonly IPage _page;

        public InPzuPricesPage(IPage page)
        {
            _page = page;
        }

        public async Task GoTo()
        {
            await _page.GotoAsync(URL);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.Locator("#terms-page1 [onclick='cookieConsentAcceptAll()']").ClickAsync();
        }

        public async Task<IEnumerable<InPzuQuote>> GetPricings(DateOnly forDate)
        {
            var lastAvailableDayText = await _page.Locator("input.date_picker.hasDatepicker").InputValueAsync();
            if (DateOnly.ParseExact(lastAvailableDayText, "dd.MM.yyyy") > forDate)
            {
                var selectOptions = new PageSelectOptionOptions { Force = true };
                await _page.ClickAsync("input.hasDatepicker");
                await _page.SelectOptionAsync("[data-handler='selectYear']", new SelectOptionValue { Value = forDate.Year.ToString() }, selectOptions);
                await _page.SelectOptionAsync("[data-handler='selectMonth']", new SelectOptionValue { Value = (forDate.Month - 1).ToString() }, selectOptions);
                await SelectDay();
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle); 
                while (_page.Url == URL)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            var categories = (await _page.Locator("select.fund_filter[data-filter-attr='unit_category'] option").EvaluateAllAsync<string[]>("(els) => els.map(x => x.getAttribute('value') + '|' + x.getAttribute('data-unit-category-code'))"))
                .Select(x =>
                {
                    var split = x.Split("|");
                    return (UnitType: split[1], Code: split[0]);
                });
            var quotes = new Dictionary<string, string>();

            var url = _page.Url;
            foreach (var category in categories)
            {
                var response = await _page.RunAndWaitForResponseAsync(async () =>
                {
                    await _page.SelectOptionAsync("select.fund_filter[data-filter-attr='unit_category']", new SelectOptionValue { Value = category.Code }, new PageSelectOptionOptions { Force = true });
                }
                , x => x.Url.Contains("component.id=") && x.Request.Method == "POST");
                var json = await response.TextAsync();
                quotes[category.UnitType] = json;
            }
            var quotesParsed = quotes.SelectMany(x =>
            {
                var quotes = JsonConvert.DeserializeObject<List<QuoteResponse>>(x.Value).Select(quote =>
                {
                    var splitValue = quote.Value.Split(" ");
                    return new InPzuQuote
                    {
                        FormattedDate = quote.FormattedDate,
                        FundName = quote.FundName,
                        UnitType = x.Key,
                        Value = double.Parse(splitValue.First()),
                        Currency = splitValue.Last()
                    };
                });
                return quotes;
            });
            return quotesParsed;
            /*
            await _page.ClickAsync("input.hasDatepicker");
            await _page.SelectOptionAsync("[data-handler='selectYear']", new SelectOptionValue { Value = forDate.Year.ToString() }, new PageSelectOptionOptions { Force = true });
            await _page.SelectOptionAsync("[data-handler='selectMonth']", new SelectOptionValue { Value = (forDate.Month - 1).ToString() }, new PageSelectOptionOptions { Force = true });


            var allFunds = await _page.Locator(".name_fund-cell").CountAsync();
            var list = new List<InPzuPrice>();
            for (int i = 1; i < allFunds; i++)
            {
                list.Add(new InPzuPrice
                {
                    FundName = await _page.Locator(".name_fund-cell").Nth(i).TextContentAsync(),
                    UnitPrice = double.Parse((await _page.Locator(".current_value-cell").Nth(i).TextContentAsync()).Split("PLN").First().Trim().Replace(",", "."))
                });
            }
            return list;
            */
            async Task SelectDay()
            {
                var dayLocator = _page.Locator($"[data-handler='selectDay'] [data-date='{forDate.Day}']");
                if (await dayLocator.CountAsync() > 0)
                {
                    await dayLocator.ClickAsync();
                    return;
                }

                if (forDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    await _page.Locator($"[data-handler='selectDay']").ClickAsync();
                    return;
                }

                if (forDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    await _page.Locator($"[data-handler='selectDay'] [data-date='{forDate.Day + 1}']").ClickAsync();
                }
                if (forDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    await _page.Locator($"[data-handler='selectDay'] [data-date='{forDate.Day - 1}']").ClickAsync();
                }
            }
        }
    }

    public class InPzuPrice
    {
        public string FundName { get; set; }
        public double UnitPrice { get; set; }
    }

    public class InPzuQuote
    {
        public string FundName { get; set; }
        public double Value { get; set; }
        public string Currency { get; set; }
        public string FormattedDate { get; set; }
        public string UnitType { get; set; }
    }

    public class QuoteResponse
    {
        public string FundName { get; set; }
        public int Risk { get; set; }
        public string UmbrellaFundName { get; set; }
        public double Change1D { get; set; }
        public double Change1M { get; set; }
        public double Change3M { get; set; }
        public double Change6M { get; set; }
        public double Change1Y { get; set; }
        public double Change2Y { get; set; }
        public double ChangeYtd { get; set; }
        public string Value { get; set; }
        public string FormattedDate { get; set; }
    }
}