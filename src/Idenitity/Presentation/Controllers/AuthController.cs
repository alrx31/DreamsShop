using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> RegisterConsumerUser([FromBody] ConsumerUserRegisterDto dto, CancellationToken cancellationToken)
    {
        await mediator.Send(new ConsumerUserRegisterCommand(dto), cancellationToken);
        
        return await LoginConsumerUser(mapper.Map<ConsumerUserLoginDto>(dto), cancellationToken);
    }

    [HttpPost]
    public async Task<IActionResult> LoginConsumerUser(ConsumerUserLoginDto dto, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ConsumerUserLoginCommand(dto), cancellationToken));    
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        return Ok();
    }
}