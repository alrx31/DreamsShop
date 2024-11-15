namespace DefaultNamespace;

public class OrderTransaction
{
    public Guid Id { get; set; }
    public Order Order { get; set; }
    public OrderTransactionStatuses Status { get; set; }
    public DateTime UpdatedAt { get; set; }
}