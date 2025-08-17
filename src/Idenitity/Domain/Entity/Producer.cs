using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Producer
{
    [Key]
    public required Guid Id { get; set; }

    [StringLength(50)]
    public required string Title { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public decimal? Rating { get; set; }

    IEnumerable<ProducerUser>? ProducerUsers { get; set; }
}
