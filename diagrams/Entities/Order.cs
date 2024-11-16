namespace Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    
    public decimal Cost { get; set; }
    
    public ConsumerUser consumer { get; set; }
    public List<DreamInOrder> Dreams { get; set; }
    
    public OrderTransaction Transaction { get; set; }
    
}