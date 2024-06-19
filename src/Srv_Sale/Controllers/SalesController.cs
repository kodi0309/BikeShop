using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Srv_Sale.Data;
using Srv_Sale.Models;
using Srv_Sale.DTOs;


namespace Srv_Sale.Controllers;

[ApiController]
[Route("api/sales")]
public class SalesController : ControllerBase
{
    private readonly SaleDbContext _context;
    private readonly IMapper _mapper;

    public SalesController(SaleDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<SaleDto>>> GetAllSales()
    {
        var sales = await _context.Sales
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Brand)
            .ToListAsync();

        return _mapper.Map<List<SaleDto>>(sales);
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

    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale(NewSaleDto saleDto)
    {
        var sale = _mapper.Map<Sale>(saleDto);

        _context.Sales.Add(sale);

        var result = await _context.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Changes not saved");

        return CreatedAtAction(nameof(GetSaleById), new { sale.Id }, _mapper.Map<SaleDto>(sale));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSale(Guid id, UpdateSaleDto updateSaleDto)
    {
        var sale = await _context.Sales.Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (sale == null) return NotFound();

        sale.Item.Brand = updateSaleDto.Brand ?? sale.Item.Brand;
        sale.Item.Model = updateSaleDto.Model ?? sale.Item.Model;
        sale.Item.Year = updateSaleDto.Year != default ? updateSaleDto.Year : sale.Item.Year;
        sale.Item.ImageUrl = updateSaleDto.ImageUrl ?? sale.Item.ImageUrl;

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();
        return BadRequest("Changes were not updated");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSale(Guid id)
    {
        var sale = await _context.Sales.FindAsync(id);

        if (sale == null) return NotFound();

        _context.Sales.Remove(sale);

        var result = await _context.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Could not delete item");

        return Ok();
    }
}