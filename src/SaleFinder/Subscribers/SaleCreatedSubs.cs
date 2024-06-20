using Transit;
using MassTransit;
using AutoMapper;
using MongoDB.Entities;

namespace SaleFinder;

public class SaleCreatedSubs : IConsumer<SaleCreated>
{
    private readonly IMapper _mapper;
    public SaleCreatedSubs(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<SaleCreated> context)
    {
        Console.WriteLine("--> Subs sale created: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        await item.SaveAsync();
    }
}