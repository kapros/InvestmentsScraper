using AutoMapper;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.Responses;
using InvestementsTracker.Services.InPzu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;

namespace InvestementsTracker.Controllers;

[ApiController, Route("portfolio")]
public class PortfolioController : InPzuController
{
    public PortfolioController(IInPzuScrapingService inpPzuScrapingService, IInPzuRepository inpzuRepository, IMapper mapper, InPzuDataContext dataContext) : base(inpPzuScrapingService, inpzuRepository, mapper, dataContext)
    {
    }

    [HttpGet("scrape")]
    public async Task<IActionResult> GetPortfolios()
    {
        return await DoTask<IActionResult>(async () =>
        {
            var portfolios = await InpPzuScrapingService.ScrapePortfolios(Page);

            return Ok(portfolios);
        });
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListPortfolios()
    {
        return await DoTask(async () =>
        {
            var portfolios = await InpzuRepository.GetPortfolios();
            var results = Mapper.Map<ListPortfoliosResponse>(portfolios);
            return Ok(results);
        });
    }
}
