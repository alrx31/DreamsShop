using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class RefreshTokenModel
{
    [Key]
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}
