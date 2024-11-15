namespace DefaultNamespace;

public class DreamInOrder
{
    public Guid Id { get; set; }
    
    public Dream Dream { get; set; }
    public Order Order { get; set; }
    public DateTime AddDate { get; set; }
    public float? Raiting { get; set; } 
    
}