using AutoMapper;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.Responses;
using InvestementsTracker.Services.InPzu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;

namespace InvestementsTracker.Controllers;

public class InPzuControllerBase : ControllerBase
{
    protected readonly InPzuDataContext DataContext;
    protected readonly IPage Page;
    protected readonly IInPzuScrapingService InpPzuScrapingService;
    protected readonly IInPzuRepository InpzuRepository;
    protected readonly IMapper Mapper;

    public InPzuControllerBase(IInPzuScrapingService inpPzuScrapingService, IInPzuRepository inpzuRepository, IMapper mapper, InPzuDataContext dataContext)
    {
        var headless = false;
        DataContext = dataContext;
        var pw = Playwright.CreateAsync().Result;
        Page = pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless }).Result.NewPageAsync().Result;
        InpPzuScrapingService = inpPzuScrapingService;
        InpzuRepository = inpzuRepository;
        Mapper = mapper;
    }



    protected async Task<TResult> DoTask<TResult>(Func<Task<TResult>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            await Page.CloseAsync();
        }
    }
}
