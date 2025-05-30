using Application.DTO;
using Application.UseCases.DreamCreate;
using Application.UseCases.DreamsGetOne;
using AutoMapper;
using Domain.Entity;
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

    [HttpGet("{dreamId:required:guid}")]
    public async Task<IActionResult> GetDream(Guid dreamId)
    {
        return Ok(await mediator.Send(mapper.Map<DreamGetOneCommand>(dreamId)));
    }
}