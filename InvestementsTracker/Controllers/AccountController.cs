using AutoMapper;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.Services.InPzu;
using Microsoft.AspNetCore.Mvc;

namespace InvestementsTracker.Controllers;
[ApiController, Route("accounts")]
public class AccountController : InPzuControllerBase
{
    public AccountController(IInPzuScrapingService inpPzuScrapingService, IInPzuRepository inpzuRepository, IMapper mapper, InPzuDataContext dataContext) : base(inpPzuScrapingService, inpzuRepository, mapper, dataContext)
    {
    }

    [HttpGet("scrape")]
    public async Task<IActionResult> ScrapeAccount()
    {
        return await DoTask<IActionResult>(async () =>
        {
            var account = await InpPzuScrapingService.ScrapeAccount(Page);
            var resp = new
            {
                Total = new
                {
                    Purchases = account.wplaty,
                    CurrentValue = account.wartosc,
                    Change = account.wynik,
                    PercentageChange = double.Round((account.wynik / account.wplaty) * 100, 2)
                },
                Funds = account.produkty.First().umowy.Select(x => new
                {
                    Registry = x.rejestrId,
                    RegistryPurchases = x.wplaty,
                    RegistryValue = x.wartosc,
                    RegistryChange = x.wynik,
                    RegistryPercentageChange = double.Round((x.wynik / x.wplaty) * 100, 2)
                })
            };
            return Ok(resp);
        });
    }
}

