using Application.DTO.ProducerUser;
using Application.UseCases.ProducerUserAuth.ProducerUserLogin;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using AutoMapper;
using MediatR;
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
    public async Task<IActionResult> RegisterProducerUser([FromBody] ProducerUserRegisterDto model)
    {
        await mediator.Send(mapper.Map<ProducerUserRegisterCommand>(model));
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> LoginProducerUser([FromBody] ProducerUserLoginDto model)
    {
        return Ok(await mediator.Send(mapper.Map<ProducerUserLoginCommand>(model)));
    }
}