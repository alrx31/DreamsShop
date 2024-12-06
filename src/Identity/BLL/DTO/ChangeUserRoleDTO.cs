using DAL.Entities;

namespace BLL.DTO;

public class ChangeUserRoleDTO
{
    public Roles role { get; set; }
    public Guid RequestorId { get; set; }
    public string Password { get; set; }
}