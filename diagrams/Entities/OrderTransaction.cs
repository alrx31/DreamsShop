namespace DefaultNamespace;

public class OrderTransaction
{
    public Guid Id { get; set; }
    public Order Order { get; set; }
    public OrderTransactionStatuses Status { get; set; }
    public DataTime UpdatedAt { get; set; }
}