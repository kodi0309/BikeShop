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
        var sales = await _context.Sales
            .Include(s => s.Item)
            .OrderBy(x => x.Item.Brand)
            .ToListAsync();

        var saleDtos = _mapper.Map<List<SaleDto>>(sales);

        return saleDtos;
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSaleById(Guid id)
    {
        var sale = await _context.Sales
            .Include(s => s.Item)
            .SingleOrDefaultAsync(s => s.Id == id);

        if (sale == null)
        {
            return NotFound();
        }

        var saleDto = _mapper.Map<SaleDto>(sale);
        return Ok(saleDto);
    }
    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale(NewSaleDto newSaleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var sale = _mapper.Map<Sale>(newSaleDto);

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            var saleDto = _mapper.Map<SaleDto>(sale);
            return CreatedAtAction(nameof(GetSaleById), new { id = sale.Id }, saleDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Save ERROR: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSale(Guid id, [FromBody] UpdateSaleDto updateSaleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var sale = await _context.Sales
            .Include(s => s.Item)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale == null)
        {
            return NotFound();
        }

        _mapper.Map(updateSaleDto, sale);

        // Ręczne mapowanie AdditionalProperties
        sale.Item.AdditionalProperties = new ItemProperties
        {
            Frame = updateSaleDto.Frame,
            Handlebar = updateSaleDto.Handlebar,
            Brakes = updateSaleDto.Brakes,
            WheelsTires = updateSaleDto.WheelsTires,
            Seat = updateSaleDto.Seat,
            DerailleursDrive = updateSaleDto.DerailleursDrive,
            AdditionalAccessories = updateSaleDto.AdditionalAccessories
        };

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SaleExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        // Opcjonalne publikowanie zdarzenia
        // await _publishEndpoint.Publish(new SaleUpdatedEvent { SaleId = sale.Id });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(Guid id)
    {
        var sale = await _context.Sales
            .Include(s => s.Item)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale == null)
        {
            return NotFound();
        }

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync();

        // Opcjonalne publikowanie zdarzenia
        // await _publishEndpoint.Publish(new SaleDeletedEvent { SaleId = id });

        return NoContent();
    }

    private bool SaleExists(Guid id)
    {
        return _context.Sales.Any(e => e.Id == id);
    }



}