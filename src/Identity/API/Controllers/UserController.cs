using BLL.DTO;
using BLL.IService;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController
    (
        IUserService userService
    ) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var user = await userService.GetUserById(userId);
     
        return Ok(user);
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDTO deleteUserDTO)
    {
        await userService.DeleteUser(deleteUserDTO);
        
        return Ok();
    }
    
    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO, Guid id)
    {
        await userService.UpdateUser(updateUserDTO, id);
        
        return Ok();
    }

    [Authorize]
    [HttpPatch("{id}/role")]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDTO updateModel, Guid id)
    {
        await userService.ChangeUserRole(updateModel, id);

        return Ok();
    }
}