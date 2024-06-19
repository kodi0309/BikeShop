using MongoDB.Entities;

namespace SaleFinder;

public class Srv_SaleHC
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public Srv_SaleHC(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }
    public async Task<List<Item>> GetItemsForFindDb()
    {
        //var allItems = await DB.Find<Item>().ExecuteAsync();

        return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"]
            + "/api/sales");
    }
}