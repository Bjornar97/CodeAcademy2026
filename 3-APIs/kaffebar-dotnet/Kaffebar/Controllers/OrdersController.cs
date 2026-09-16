using System.ComponentModel.DataAnnotations;
using Kaffebar.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Kaffebar.Controllers;

[ApiController]
public class OrdersController : ControllerBase
{
    [HttpGet("/orders")]
    [ProducesResponseType(200)]
    public ActionResult<List<Order>> GetOrders([FromQuery] OrderQuery query)
    {
        return Ok(
            new List<Order>
            {
                new(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Pending, DateTime.UtcNow),
                new(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Brewing, DateTime.UtcNow),
                new(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Ready, DateTime.UtcNow),
                new(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Pending, DateTime.UtcNow),
                new(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Brewing, DateTime.UtcNow),
                new(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Ready, DateTime.UtcNow),
            }
                .Where(o => !query.Status.HasValue || o.Status == query.Status.Value)
                .Skip(query.Offset)
                .Take(query.Limit)
        );
    }

    [HttpPost("/orders")]
    public Order CreateOrder(CreateOrderRequest request)
    {
        return new Order(Guid.NewGuid(), request.CoffeeId, OrderStatus.Pending, DateTime.UtcNow);
    }

    [HttpGet("/orders/{orderId:guid:minlength(3)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public ActionResult<Order> GetOrder(Guid orderId)
    {
        if (orderId != Guid.Parse("a839b873-8a21-4bbb-8760-e923ecad71ff"))
        {
            return NotFound();
        }

        return Ok(new Order(orderId, Guid.NewGuid(), OrderStatus.Pending, DateTime.UtcNow));
    }

    [HttpPost("/orders/{orderId:guid:minlength(3)}/status")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public ActionResult<Order> UpdateOrderStatus(UpdateOrderStatusRequest request, Guid orderId)
    {
        if (orderId != Guid.Parse("a839b873-8a21-4bbb-8760-e923ecad71ff"))
        {
            return NotFound();
        }

        return Ok(new Order(orderId, Guid.NewGuid(), request.Status, DateTime.UtcNow));
    }
}
