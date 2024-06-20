using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Srv_Sale.Data;
using Srv_Sale.Models;
using Srv_Sale.DTOs;
using Transit;
using MassTransit;


namespace Srv_Sale.Controllers;

[ApiController]
[Route("api/sales")]
public class SalesController : ControllerBase
{
    private readonly SaleDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public SalesController(SaleDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<SaleDto>>> GetAllSales()
    {
        var query = _context.Sales.OrderBy(x => x.Item.Brand).AsQueryable();

        return await query.ProjectTo<SaleDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSaleById(Guid id)
    {
        var sale = await _context.Sales
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (sale == null) return NotFound();

        return _mapper.Map<SaleDto>(sale);
    }
    //[Authorize]
    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale(NewSaleDto saleDto)
    {
        var sale = _mapper.Map<Sale>(saleDto);

        //Seller
        //sale.Seller = User.Identity.Name; //required, if login there is a name

        _context.Sales.Add(sale);

        var newSale = _mapper.Map<SaleDto>(sale);

        await _publishEndpoint.Publish(_mapper.Map<SaleCreated>(newSale));

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Changes not saved");

        return CreatedAtAction(nameof(GetSaleById), new { sale.Id }, newSale);
    }

    //[Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSale(Guid id, UpdateSaleDto updateSaleDto)
    {
        var sale = await _context.Sales.Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (sale == null) return NotFound();

        //Seller
        //sale.Seller = User.Identity.Name; //required, if login there is a name

        sale.Item.Brand = updateSaleDto.Brand ?? sale.Item.Brand;
        sale.Item.Model = updateSaleDto.Model ?? sale.Item.Model;
        sale.Item.Year = updateSaleDto.Year != default ? updateSaleDto.Year : sale.Item.Year;
        sale.Item.ImageUrl = updateSaleDto.ImageUrl ?? sale.Item.ImageUrl;

        await _publishEndpoint.Publish(_mapper.Map<SaleUpdated>(sale));

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();
        return BadRequest("Changes were not updated");
    }

    //[Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSale(Guid id)
    {
        var sale = await _context.Sales.FindAsync(id);

        if (sale == null) return NotFound();

        //Seller
        //sale.Seller = User.Identity.Name; //required, if login there is a name
        
        _context.Sales.Remove(sale);

        await _publishEndpoint.Publish<SaleDeleted>(new { Id = sale.Id.ToString() });

        var result = await _context.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Could not delete item");

        return Ok();
    }
}