namespace DefaultNamespace;

public class Produser
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public List<ProduserUser> Staff { get; set; }
    
    public float? Raiting { get; set; }
    
    
}