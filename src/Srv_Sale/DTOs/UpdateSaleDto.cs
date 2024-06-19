using System.ComponentModel.DataAnnotations;

namespace Srv_Sale.DTOs;

public class UpdateSaleDto
{
    //Sale.cs
    [Required]
    public int StartPrice { get; set; }

    //Item.cs
    [Required]
    public string Brand { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public string ImageUrl { get; set; }
}
