using AutoMapper;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.Services.InPzu;
using Microsoft.AspNetCore.Mvc;

namespace InvestementsTracker.Controllers;
[ApiController, Route("accounts")]
public class AccountController : InPzuController
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
                    account.wplaty,
                    account.wartosc,
                    account.wynik
                },
                Funds = account.produkty.First().umowy.Select(x => new
                {
                    x.rejestrId,
                    x.wplaty,
                    x.wartosc,
                    x.wynik
                })
            };
            return Ok(resp);
        });
    }
}

