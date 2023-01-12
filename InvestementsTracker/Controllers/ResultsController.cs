using AutoMapper;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.Services.InPzu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestementsTracker.Controllers;

[ApiController, Route("results")]
public class ResultsController : InPzuControllerBase
{
    public ResultsController(IInPzuScrapingService inpPzuScrapingService, IInPzuRepository inpzuRepository, IMapper mapper, InPzuDataContext dataContext) : base(inpPzuScrapingService, inpzuRepository, mapper, dataContext)
    {
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllResults()
    {
        return await DoTask(async () =>
        {
            var list = await DataContext.InPzuOrders.Include(x => x.Positions).ThenInclude(x => x.Results).Where(x => x.OrderType == OrderType.Purchase).ToListAsync();
            return Ok(list);
        });
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestResults()
    {
        return await DoTask(async () =>
        {
            var ret = await InpPzuScrapingService.GetResultsForDate(DateOnly.FromDateTime(DateTime.Today), Page);
            return Ok(ret);
        });
    }
}

