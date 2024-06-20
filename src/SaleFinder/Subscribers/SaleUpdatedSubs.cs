using AutoMapper;
using Transit;
using MassTransit;
using MongoDB.Entities;

namespace SaleFinder;

public class SaleUpdatedSubs : IConsumer<SaleUpdated>
{
    private readonly IMapper _mapper;

    public SaleUpdatedSubs(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<SaleUpdated> context)
    {
        Console.WriteLine("--> Consuming sale updated: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        var result = await DB.Update<Item>()
            .Match(a => a.ID == context.Message.Id)
            .ModifyOnly(x => new
            {
                x.Brand,
                x.Model,
                x.Year,
                x.ImageUrl,
                x.StartPrice
            }, item)
            .ExecuteAsync();

        if (!result.IsAcknowledged) 
            throw new MessageException(typeof(SaleUpdated), "Problem updating mongodb");
    }
}
