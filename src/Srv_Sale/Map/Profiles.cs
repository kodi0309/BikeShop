using AutoMapper;
using Srv_Sale.Models;
using Srv_Sale.DTOs;

namespace Srv_Sale.Map;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<Sale, SaleDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, SaleDto>();
        CreateMap<NewSaleDto, Sale>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<NewSaleDto, Item>();
    }
}