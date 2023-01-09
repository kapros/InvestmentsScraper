using Microsoft.Playwright;
using System.Linq;
using static InPzuScraper.InPzuHistory;

namespace InvestementsTracker.InPzuScraper
{
    public class InPzuMyOrdersPage
    {
        private IPage _page;
        public History? History { get; private set; }

        public InPzuMyOrdersPage(IPage page)
        {
            _page = page;
        }

        public async Task GoTo()
        {
            var response = await _page.RunAndWaitForResponseAsync(async () =>
            {
                await _page.GotoAsync("https://inpzu.pl/tfi/moja-historia-zlecen/zrealizowane");
            }, x => x.Url.Contains("dane") && x.Request.PostData.Contains("ZLEC-HISTORIA"));
            var rawText = await response.TextAsync();
            History = new History(rawText);
        }

        public async Task<IEnumerable<InPzuOrders>> GetOrdersSince(DateOnly since)
        {
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.Locator("input.input.filtrOd").ClickAsync();
            await SetDate(since);
            await _page.Locator("input.input.filtrDo").ClickAsync();
            await SetDate(DateOnly.FromDateTime(DateTime.Today));
            Thread.Sleep(1000);
            var xpath = "//app-moja-historia-zlecen-zrealizowane//div[contains(@class, 'collapsible-container') and contains(@class, 'ng-star-inserted')]";
            var locator = _page.Locator(xpath);
            var elements = await locator.CountAsync();
            var orders = new List<InPzuOrders>();
            for (int i = 0; i < elements; i++)
            {
                await locator.Nth(i).ClickAsync();
               
                var order = new InPzuOrders();
                orders.Add(order);
                var header = locator.Nth(i).Locator("xpath=//div/div >> nth=0");
                var date = await header.Nth(0).Locator("xpath=//div/p").Nth(0).TextContentAsync();
                order.Date = DateOnly.ParseExact(date, " dd.mm.yyyy ");
                order.Name = (await header.Nth(0).Locator("xpath=//div[2]/p").Nth(0).TextContentAsync()).Trim();
                order.Status = ConvertStatus(await header.Nth(0).Locator("xpath=//div[3]/p").Nth(0).TextContentAsync());
                var rows = locator.Nth(i).Locator("xpath=//*[contains(@class, 'collapsible-contents-invis')]");
                var rowsCount = Math.Max(await rows.CountAsync() - 1, 1);
                var shouldUnfold = rowsCount > 1;
                for (int itemRow = 0; itemRow < rowsCount; itemRow++)
                {
                    if (shouldUnfold)
                        await rows.Nth(itemRow).ClickAsync();
                    var details = locator.Nth(i).Locator(shouldUnfold ? "xpath=//*[contains(@class, 'accordion-container-open')]" : "xpath=//div[@app-historia-zlecenie-finansowe]");
                    var fundName = await details.Locator(LocatorForDetails("Nazwa subfunduszu")).TextContentAsync();
                    var priceOfUnit = await details.Locator(LocatorForDetails("Cena jednostki")).TextContentAsync();
                    var purchasedUnits = await details.Locator(LocatorForDetails("Liczba jednostek")).TextContentAsync();
                    var purchaseValue = await details.Locator(LocatorForDetails("Kwota brutto")).TextContentAsync();
                    var fundPurchase = new FundPurchase
                    {
                        Name = fundName,
                        PricePerUnit = double.Parse(priceOfUnit.Replace("PLN", string.Empty).Trim().Replace(",", ".")),
                        PurchasedUnits = double.Parse(purchasedUnits.Replace("+", string.Empty).Trim().Replace(",", ".")),
                        PurchaseValue = double.Parse(purchaseValue.Replace("+", string.Empty).Replace("PLN", string.Empty).Trim().Replace(",", "."))
                    };
                    order.Funds.Add(fundPurchase);
                    if (shouldUnfold)
                        await rows.Nth(itemRow).Locator("xpath=//div/div").Nth(0).ClickAsync();
                }
            }

            return orders;

            string LocatorForDetails(string detailName) => $"xpath=//p[contains(text(), '{detailName}')]/../following-sibling::div[1]";

            async Task SetDate(DateOnly date)
            {
                var firstDayOfMonthButton = _page.Locator("button.pika-button.pika-day").Last;
                var month = await firstDayOfMonthButton.GetAttributeAsync("data-pika-month");
                var year = await firstDayOfMonthButton.GetAttributeAsync("data-pika-year");
                var day = await firstDayOfMonthButton.GetAttributeAsync("data-pika-day");
                var currentDate = new DateOnly(int.Parse(year), int.Parse(month) + 1, int.Parse(day));
                if (currentDate.Year != date.Year)
                {
                    await _page.Locator("select.pika-select-year").Last.ClickAsync();
                    await _page.Locator("select.pika-select-year").Last.SelectOptionAsync(
                        new[]
                        {
                            new SelectOptionValue
                            {
                                Value = year.ToString()
                            }
                        },
                        new LocatorSelectOptionOptions
                        {
                            Force = true
                        });
                }
                if (currentDate.Month != date.Month)
                {
                    await _page.Locator("select.pika-select-month").Last.ClickAsync();
                    await _page.Locator("select.pika-select-month").Last.SelectOptionAsync(
                        new[]
                        {
                            new SelectOptionValue
                            {
                                Value = (date.Month - 1).ToString()
                            }
                        },
                        new LocatorSelectOptionOptions
                        {
                            Force = true
                        });
                }
                if (currentDate.Day != date.Day)
                {
                    await _page.Locator($"[data-pika-day='{date.Day}']").Last.ClickAsync();
                }
            }
        }

        private static string ConvertStatus(string status) => 
            status.Trim() == "DOPŁATA (nabycie)" ? "Purchase" : "Sold";
    }

    public class InPzuOrders
    {
        public DateOnly Date { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public IList<FundPurchase> Funds { get; set; } = new List<FundPurchase>();
    }

    public class FundPurchase
    {
        public string Name { get; set; }
        public double PricePerUnit { get; set; }
        public double PurchasedUnits { get; set; }
        public double PurchaseValue { get; set; }
    }
}