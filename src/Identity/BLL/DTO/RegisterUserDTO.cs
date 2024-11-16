namespace BLL.DTO;

public class RegisterUserDTO
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
}