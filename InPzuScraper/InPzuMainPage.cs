using Microsoft.Playwright;

namespace InvestementsTracker.InPzuScraper
{
    public class InPzuMainPage
    {
        private readonly IPage _page;

        public InPzuMainPage(IPage page)
        {
            _page = page;
        }

        public async Task<InPzuMyOrdersPage> GoToOrders()
        { 
            var ordersPage = new InPzuMyOrdersPage(_page);
            await ordersPage.GoTo();
            return ordersPage;
        }
    }
}