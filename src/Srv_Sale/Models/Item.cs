//Update SaleDto.cs

using System.ComponentModel.DataAnnotations.Schema;
using Srv_Sale.Models;

namespace Srv_Sale;

[Table("Items")]
public class Item
{
    public Guid Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; }

    public Sale Sale { get; set; }
    public Guid SaleId { get; set; }
}