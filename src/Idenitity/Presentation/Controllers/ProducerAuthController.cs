using Application.DTO.ProducerUser;
using Application.UseCases.ProducerUserAuth.ProducerUserLogin;
using Application.UseCases.ProducerUserAuth.ProducerUserRefreshAccessToken;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using AutoMapper;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProducerAuthController(
    IMediator mediator,
    IMapper mapper
    ) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> RegisterProducerUser([FromBody] ProducerUserRegisterDto model, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<ProducerUserRegisterCommand>(model), cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> LoginProducerUser([FromBody] ProducerUserLoginDto model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(mapper.Map<ProducerUserLoginCommand>(model), cancellationToken));
    }

    [HttpPatch]
    [Authorize(Roles = nameof(Roles.Provider))]
    public async Task<IActionResult> RefreshAccessToken(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ProducerUserRefreshAccessTokenCommand(), cancellationToken));
    }
}