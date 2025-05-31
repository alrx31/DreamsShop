namespace Domain.Entity;

public interface IHasClaims
{
    Guid Id { get; set; }
    Roles Role { get; set; }
}