using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Category
{
    [Key]
    public required Guid CategoryId { get; init; }
    [StringLength(100)]
    public required string Title { get; set; }
    [StringLength(1000)]
    public string? Description { get; set; }
        
    public IEnumerable<DreamCategory>? DreamCategories { get; set; }
}