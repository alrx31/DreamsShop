namespace Domain.Entity;

public class DreamCategory
{
    public required Guid DreamCategoryId { get; set; }
    public Guid Dreams { get; set; }
    public Guid Categories { get; set; }
}