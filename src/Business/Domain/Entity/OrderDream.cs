using System.Text.Json.Serialization;

namespace Domain.Entity;

public class OrderDream
{
    public Guid OrderId { get; set; }
    public Guid DreamId { get; set; }

    [JsonIgnore]
    public Dream? Dream { get; set; }
    [JsonIgnore]
    public Order? Order { get; set; }
}
