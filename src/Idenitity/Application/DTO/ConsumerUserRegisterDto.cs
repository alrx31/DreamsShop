using Domain.Entity;

namespace Application.DTO;

public class ConsumerUserRegisterDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string PasswordRepeat { get; init; }
    public required string Name { get; init; }
}