namespace Kaffebar.Models;

public record Order(Guid Id, Guid ItemId, OrderStatus Status, DateTime CreatedAt);
