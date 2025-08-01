using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class ConsumerUser : UserData, IHasClaims
{
    [StringLength(50)]
    public required string Password { get; set; }
}
