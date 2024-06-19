namespace Srv_Sale.DTOs;

public class SaleDto
{
    //Sale.cs
    public Guid Id { get; set; }
    public int StartPrice { get; set; }
    public string Status { get; set; }

    //Item.cs
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; }
}
