using DB.TemporalTable.Api.Context;
using DB.TemporalTable.Api.Entities;
using DB.TemporalTable.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DB.TemporalTable.Api.Controllers
{

    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        public readonly StoreContext _storeContext;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(StoreContext storeContext, IHttpContextAccessor httpContextAccessor)
        {
            _storeContext = storeContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequest orderRequest)
        {
            var order = new Order
            {
                Description = orderRequest.Description,
                Name = orderRequest.Name,
                Quantity = orderRequest.Quantity,
                UnitPrice = orderRequest.UnitPrice,
                CreatedBy = _httpContextAccessor.HttpContext.Request.Headers["Current-User"].ToString()
            };

            await _storeContext.Order.AddAsync(order);
            await _storeContext.SaveChangesAsync();

            return Created();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch([FromBody] OrderRequest orderRequest, [FromRoute(Name = "id")] int id)
        {
            var order = await _storeContext.Order.SingleAsync(s => s.Id == id);

            order.Description = orderRequest.Description;
            order.Name = orderRequest.Name;
            order.Quantity = orderRequest.Quantity;
            order.UnitPrice = orderRequest.UnitPrice;
            order.CreatedBy = _httpContextAccessor.HttpContext.Request.Headers["Current-User"].ToString();
            await _storeContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHistoricalById([FromRoute(Name = "id")] int id)
        {
            var orders = await _storeContext.Order.TemporalAll()
                .Where(t => t.Id == id)
                .OrderByDescending(emp => EF.Property<DateTime>(emp, "PeriodStart"))
                .Select(order => new OrderResponse(
                    order.Id,
                    order.Name,
                    order.Description,
                    order.UnitPrice,
                    order.Quantity,
                    order.RowTotal,
                  EF.Property<DateTime>(order, "PeriodStart"),
                  EF.Property<DateTime>(order, "PeriodEnd"))
                ).ToListAsync();

            return Ok(orders);
        }
    }
}