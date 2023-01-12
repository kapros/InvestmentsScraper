using AutoMapper;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.Responses;
using InvestementsTracker.Services.InPzu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;

namespace InvestementsTracker.Controllers;

[ApiController, Route("orders")]
public class OrdersController : InPzuControllerBase
{
    public OrdersController(IInPzuScrapingService inpPzuScrapingService, IInPzuRepository inpzuRepository, IMapper mapper, InPzuDataContext dataContext) : base(inpPzuScrapingService, inpzuRepository, mapper, dataContext)
    {
    }

    [HttpGet("allpurchases")]
    public async Task<IActionResult> GetAllOrders()
    {
        return await DoTask(async () =>
        {
            var inPzuOrders = await InpPzuScrapingService.GetOrders(Page);
            return Ok(inPzuOrders);
        });
    }
}
