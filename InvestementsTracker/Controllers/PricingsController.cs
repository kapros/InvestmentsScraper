using AutoMapper;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.Services.InPzu;
using Microsoft.AspNetCore.Mvc;

namespace InvestementsTracker.Controllers;

    [ApiController, Route("pricings")]
    public class PricingsController : InPzuControllerBase
    {
    public PricingsController(IInPzuScrapingService inpPzuScrapingService, IInPzuRepository inpzuRepository, IMapper mapper, InPzuDataContext dataContext) : base(inpPzuScrapingService, inpzuRepository, mapper, dataContext)
    {
    }

    [HttpGet("for")]
        public async Task<IActionResult> GetForDate(DateTime forDate)
        {
            return await DoTask(async () =>
            {
                var ret = await InpPzuScrapingService.GetResultsForDate(DateOnly.FromDateTime(forDate), Page);
                return Ok(ret);
            });
        }
    
}
