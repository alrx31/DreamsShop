namespace Application.DTO.ConsumerUser;

public class ConsumerUserLoginDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}