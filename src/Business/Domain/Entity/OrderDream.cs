using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class OrderDream
{
    public Guid OrderId { get; set; }
    public Guid DreamId { get; set; }

    public Dream? Dream { get; set; }
    public Order? Order { get; set; }
}
