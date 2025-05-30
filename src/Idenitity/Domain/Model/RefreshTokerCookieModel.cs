namespace Domain.Model;

public class RefreshTokerCookieModel
{
    public DateTime Expires { get; set; }
    public string? Token { get; set; }
}