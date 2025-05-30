using Application.DTO;
using Application.UseCases.CommandHandler;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller;

[ApiController]
[Route("api/[controller]")]
public class DreamController(
    IMediator mediator,
    IMapper mapper
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDream([FromBody] DreamCreateDto model)
    {
        await mediator.Send(mapper.Map<DreamCreateCommand>(model));
        return Ok();
    }
}