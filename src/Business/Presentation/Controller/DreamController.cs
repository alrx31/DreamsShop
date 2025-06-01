using Application.DTO;
using Application.UseCases.Dreams.DreamCreate;
using Application.UseCases.Dreams.DreamDelete;
using Application.UseCases.Dreams.DreamGetAll;
using Application.UseCases.Dreams.DreamGetCount;
using Application.UseCases.Dreams.DreamsGetOne;
using Application.UseCases.Dreams.DreamUpdate;
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
    [Authorize]
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
    public async Task<IActionResult> GetAllDreams(int skip = 0, int take = 5)
    {
        return Ok(await mediator.Send(
            mapper.Map<DreamGetAllCommand>( (skip, take) )
            )
        );
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetDreamCount()
    {
        return Ok(await mediator.Send(new DreamGetCountCommand()));
    }

    [HttpDelete("{dreamId:required:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteDream(Guid dreamId)
    {
        await mediator.Send(mapper.Map<DreamDeleteCommand>(dreamId));
        return Ok();
    }

    [HttpPut("{dreamId:required:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateDream(Guid dreamId, [FromBody] DreamUpdateDto model)
    {
        await mediator.Send(
            mapper.Map<DreamUpdateCommand>( (dreamId,model) )
            );
        return Ok();
    }
}