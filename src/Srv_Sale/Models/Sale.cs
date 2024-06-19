//Update SaleDto.cs

namespace Srv_Sale.Models;

public class Sale
{
    public Guid Id { get; set; }
    public int StartPrice { get; set; } = 0;
    public Status Status { get; set; }
    public Item Item { get; set; }
}