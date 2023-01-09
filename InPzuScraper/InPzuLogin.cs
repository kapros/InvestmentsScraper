using Microsoft.Playwright;

namespace InvestementsTracker.InPzuScraper
{
    public class InPzuLogin
    {
        private readonly IPage _page;
        private const string URL = "https://inpzu.pl/tfi/login";

        public InPzuLogin(IPage page)
        {
            _page = page;
        }

        public async Task<InPzuMainPage> Login(string username, string password)
        {
            await _page.GotoAsync(URL);
            await _page.Locator("#terms-pageStd .btn-cta").ClickAsync();
            await _page.Locator("#username").TypeAsync(username);
            await _page.Locator("#password").TypeAsync(password);
            await _page.Locator("#zaloguj").ClickAsync();
            await _page.WaitForNavigationAsync();
            return new InPzuMainPage(_page);
        }
    }
}
