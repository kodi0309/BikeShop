using Srv_Sale.Models;
using Microsoft.EntityFrameworkCore;

namespace Srv_Sale.Data;
public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<SaleDbContext>());
    }

    private static void SeedData(SaleDbContext context)
    {
        context.Database.Migrate();

        if (context.Sales.Any())
        {
            Console.WriteLine("No need to seed");
            return;
        }

        var sales = new List<Sale>()
        {
            // 1 Santa Cruz Hightower 2
            new Sale
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.InSale,
                StartPrice = 6500,
                Item = new Item
                {
                    Brand = "Santa Cruz",
                    Model = "Hightower 2",
                    Year = 2024,
                    ImageUrl = "https://piwnicarowerowa.pl/img/25576/santa-cruz-hightower-2-aluminium-29-smoke-grey-d"
                }
            },
	        // 2 Santa Cruz Heckler
            new Sale
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a1c"),
                Status = Status.InSale,
                StartPrice = 11000,
                Item = new Item
                {
                    Brand = "Santa Cruz",
                    Model = "Heckler",
                    Year = 2024,
                    ImageUrl = "https://piwnicarowerowa.pl/img/44001/rower-elektryczny-santa-cruz-heckler-sl-r-carbon-c"
                }
            }

        };

        context.AddRange(sales);
        context.SaveChanges();
    }
}