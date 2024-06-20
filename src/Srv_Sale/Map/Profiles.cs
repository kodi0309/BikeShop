using AutoMapper;
using Srv_Sale.Models;
using Srv_Sale.DTOs;
using Transit;

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
        CreateMap<SaleDto, SaleCreated>();
        CreateMap<Sale, SaleUpdated>().IncludeMembers(a => a.Item);
        CreateMap<Item, SaleUpdated>();
    }
}