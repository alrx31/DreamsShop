namespace Domain.Entity;

public class ProducerUser : UserData, IHasClaims
{
    public required Guid ProducerId { get; set; }
    
    public Producer? Producer { get; set; }
}
