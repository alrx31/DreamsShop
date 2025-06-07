using Application.UseCases.UserDream.UserDreamAdd;
using Application.UseCases.UserDream.UserDreamDelete;
using AutoMapper;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserDream(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost("{dreamId:required:guid}")]
    [Authorize(Roles = nameof(Roles.Consumer))]
    public async Task<IActionResult> AddDreamToUser(Guid dreamId)
    {
        await mediator.Send(mapper.Map<UserDreamAddCommand>(dreamId));
        return Ok();
    }

    [HttpDelete("{dreamId:required:guid}")]
    [Authorize(Roles = nameof(Roles.Consumer))]
    public async Task<IActionResult> DeleteDreamFromUser(Guid dreamId)
    {
        await mediator.Send(mapper.Map<UserDreamDeleteCommand>(dreamId));
        return Ok();
    }
}