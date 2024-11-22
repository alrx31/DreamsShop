namespace BLL.DTO;

public class DeleteUserDTO
{
    public Guid userId { get; set; }
    public Guid requestorId { get; set; }
    
    public string password { get; set; }
}