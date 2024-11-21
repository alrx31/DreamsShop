namespace DAL.Entities;

public class Producer
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public List<User> Staff { get; set; }
    
    public float? Raiting { get; set; }
    
    
}