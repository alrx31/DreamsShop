namespace Domain.Model;

public class RefreshTokenCookieModel
{
    public DateTime Expires { get; set; }
    public string? Token { get; set; }
}