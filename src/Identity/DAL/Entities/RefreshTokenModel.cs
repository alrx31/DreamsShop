namespace DAL.Entities;

public class RefreshTokenModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string? Token { get; set; }
    public DateTime? ExpiryTime { get; set; }
}