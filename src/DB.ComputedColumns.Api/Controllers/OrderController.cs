using DB.TemporalTable.Api.Context;
using DB.TemporalTable.Api.Entities;
using DB.TemporalTable.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DB.TemporalTable.Api.Controllers
{

    [ApiController]
    [Route("orders")]
    public class OrderController(StoreContext storeContext, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        public readonly StoreContext _storeContext = storeContext;
        private readonly string _currentUser = httpContextAccessor?.HttpContext?.Request?.Headers["Current-User"];

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequest orderRequest)
        {
            var order = new Order
            {
                Product = orderRequest.Product,
                Description = orderRequest.Description,
                Quantity = orderRequest.Quantity,
                UnitPrice = orderRequest.UnitPrice,
                CreatedBy = _currentUser
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
            order.Product = orderRequest.Product;
            order.Quantity = orderRequest.Quantity;
            order.UnitPrice = orderRequest.UnitPrice;
            order.CreatedBy = _currentUser;

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
                    order.Product,
                    order.Description,
                    order.UnitPrice,
                    order.Quantity,
                    order.CreatedBy,
                  EF.Property<DateTime>(order, "PeriodStart"),
                  EF.Property<DateTime>(order, "PeriodEnd")
                  )
                ).ToListAsync();

            return Ok(orders);
        }
    }
}