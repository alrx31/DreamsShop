using Application.DTO;
using Application.DTO.ConsumerUser;
using Application.UseCases.ConsumerUserAuth.ConsumerUserLogin;
using Application.UseCases.ConsumerUserAuth.ConsumerUserRegister;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsumerAuthController(IMediator mediator) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> RegisterConsumerUser([FromBody] ConsumerUserRegisterDto dto, CancellationToken cancellationToken)
    {
        await mediator.Send(new ConsumerUserRegisterCommand(dto), cancellationToken);
        
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> LoginConsumerUser([FromBody] ConsumerUserLoginDto dto, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ConsumerUserLoginCommand(dto), cancellationToken));    
    }
}