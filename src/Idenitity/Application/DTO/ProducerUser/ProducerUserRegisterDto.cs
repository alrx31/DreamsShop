namespace Application.DTO.ProducerUser;

public class ProducerUserRegisterDto
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Password { get; init; }
    public required string PasswordRepeat { get; init; }
}