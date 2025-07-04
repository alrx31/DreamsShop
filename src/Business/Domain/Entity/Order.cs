using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Order
{
    [Key]
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public IEnumerable<OrderDream>? OrderDreams { get; set; }
}
