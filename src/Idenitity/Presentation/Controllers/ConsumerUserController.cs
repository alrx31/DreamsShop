using Application.UseCases.ConsumerUser;
using Application.UseCases.ConsumerUser.ConsumerUserDelete;
using AutoMapper;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsumerUserController (IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpDelete("{userId:required:guid}")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> DeleteUser(Guid userId, CancellationToken cancellationToken)
    {
        await mediator.Send(
            mapper.Map<ConsumerUserDeleteCommand>(userId)
            , cancellationToken);
        return Ok();
    }
}