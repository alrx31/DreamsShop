using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class ProducerUser : UserData, IHasClaims
{
    public required Guid ProducerId { get; set; }
    
    [StringLength(50)]
    public required string Password { get; set; }
    
    public Producer? Producer { get; set; }
}
