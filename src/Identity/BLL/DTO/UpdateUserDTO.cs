namespace BLL.DTO;

public class UpdateUserDTO
{
    public RegisterUserDTO UserDTO { get; set; }
    public Guid RequestorId { get; set; }
    public string Password { get; set; }
}