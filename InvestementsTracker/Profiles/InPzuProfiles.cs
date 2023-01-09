using AutoMapper;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.Responses;

namespace InvestementsTracker.Profiles;

public class InPzuProfiles : Profile
{
    public InPzuProfiles()
    {
        CreateMap<IGrouping<string, InPzuPosition>, InPzuPositionResponse>()
            .ForMember(x => x.SumOfPurchases, x => x.MapFrom(ps => ps.Sum(pos => pos.PurchaseValue)))
            .ForMember(x => x.FundName, x => x.MapFrom(y => y.Key));
        CreateMap<InPzuPortfolio, ListPortfoliosResponse>()
            .ForMember(x => x.SumPurchased, x => x.MapFrom(y => y.Positions.Sum(pos => pos.PurchaseValue)))
            .ForMember(x => x.TotalChange, x => x.MapFrom(y => y.Positions.Sum(pos => pos.Results.OrderByDescending(r => r.Date).First().Value)));
    }
}
