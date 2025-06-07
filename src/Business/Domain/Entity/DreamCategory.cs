namespace Domain.Entity;

public class DreamCategory
{
    public Guid DreamId { get; set; }
    public Guid CategoryId { get; set; }
    
    public Dream Dream { get; set; }
    public Category Category { get; set; }
}