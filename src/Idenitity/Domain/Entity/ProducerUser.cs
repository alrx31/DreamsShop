using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class ProducerUser : UserData, IHasClaims
{
    [StringLength(50)]
    public required string Password { get; set; }
    
    public required Guid ProducerId { get; set; }
}