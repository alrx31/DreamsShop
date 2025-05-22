using Application.DTO;
using Application.UseCases.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterUser(ConsumerUserRegisterDto dto, CancellationToken cancellationToken)
    {
        await mediator.Send(new ConsumerUserRegisterCommand(dto), cancellationToken);
        
        return Ok();
    }
}