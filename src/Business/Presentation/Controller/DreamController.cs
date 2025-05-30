using Application.DTO;
using Application.UseCases.DreamCreate;
using Application.UseCases.DreamDelete;
using Application.UseCases.DreamGetAll;
using Application.UseCases.DreamGetCount;
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
        return Ok(await mediator.Send(
            mapper.Map<DreamGetOneCommand>(dreamId)
            )
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDreams(int skip, int take)
    {
        return Ok(await mediator.Send(
            mapper.Map<DreamGetAllCommand>( (skip, take) )
            )
        );
    }

    [HttpGet("/count")]
    public async Task<IActionResult> GetDreamCount()
    {
        return Ok(await mediator.Send(new DreamGetCountCommand()));
    }

    [HttpDelete("{dreamId:required:guid}")]
    public async Task<IActionResult> DeleteDream(Guid dreamId)
    {
        await mediator.Send(mapper.Map<DreamDeleteCommand>(dreamId));
        return Ok();
    }
}