namespace Domain.Entity;

public interface IHasClaims
{
    string Email { get; set; }
    Roles Role { get; set; }
}