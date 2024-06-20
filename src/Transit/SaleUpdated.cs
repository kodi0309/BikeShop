namespace Transit;

public class SaleUpdated
{
    public string Id { get; set; }

    //Sale.cs
    public int StartPrice { get; set; }

    //Item.cs
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; }
}