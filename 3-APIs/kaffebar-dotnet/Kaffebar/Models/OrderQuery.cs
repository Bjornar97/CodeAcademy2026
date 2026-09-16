using System.ComponentModel.DataAnnotations;

namespace Kaffebar.Models;

public record OrderQuery(OrderStatus? Status, [Range(1, 100)] int Limit = 20, int Offset = 0);
