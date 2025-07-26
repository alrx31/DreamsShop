namespace Application.DTO.Order;

public class OrderResponseDto
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public IEnumerable<OrderDreamDto>? OrderDreams { get; set; }
}
