using System.ComponentModel.DataAnnotations;

namespace Kaffebar.Models;

public record CreateOrderRequest(
    Guid ItemId,
    CoffeeSize Size,
    MilkType? Milk,
    bool? ExtraShot,
    [Required] [StringLength(50, MinimumLength = 2)] string CustomerName,
    [Required] [Range(1, 10)] int Quantity
);
