namespace Kaffebar.Models;

public record Coffee(Guid Id, string Name, decimal Price) : OrderItem(Id, Name, Price);
