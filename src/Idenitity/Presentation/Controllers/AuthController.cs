using System.Text.Json;
using System.Xml;
using Application.DTO;
using Application.Exceptions;
using Application.UseCases.ConsumerUserLogin;
using Application.UseCases.ConsumerUserRegister;
using AutoMapper;
using Domain.IServices;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
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
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        return Ok();
    }
}