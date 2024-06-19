using MongoDB.Entities;

namespace SaleFinder;


public class Item : Entity
{
    //public string ID { get; set; }

    //Sale.cs
    public int StartPrice { get; set; }
    public string Status { get; set; }

    //Item.cs
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; }
}