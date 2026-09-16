namespace Kaffebar.Models;

public record Pastry(Guid Id, string Name, decimal Price, bool IsVegan)
    : OrderItem(Id, Name, Price);
