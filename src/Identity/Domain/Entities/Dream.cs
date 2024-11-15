namespace Domain.Entities;

public class Dream
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Media Image { get; set; }
    public Media Preview { get; set; }
    public ProduserUser Producer { get; set; }
    public float? Raiting { get; set; }
    
    
    public List<DreamInOrder> DreamInOrders { get; set; }
}