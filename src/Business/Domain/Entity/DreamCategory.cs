namespace Domain.Entity;

public class DreamCategory
{
    public required Guid DreamCategoryId { get; set; }
    public List<Dream> Dreams { get; set; }
    public List<Category> Categories { get; set; }
}