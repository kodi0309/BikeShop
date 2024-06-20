using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using SaleFinder;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<Srv_SaleHC>().AddPolicyHandler(GetPolicy());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<SaleCreatedSubs>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint("search-sale-created-subs", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 5));

            e.ConfigureConsumer<SaleCreatedSubs>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }

});

app.Run();

// Retry get data from Srv_Sales to Sale Finder
static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
