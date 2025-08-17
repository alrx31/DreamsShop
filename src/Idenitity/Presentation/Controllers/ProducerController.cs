using Application.DTO.Producer;
using Application.UseCases.Producer.ProducerCreate;
using Application.UseCases.Producer.ProducerDelete;
using Application.UseCases.Producer.ProducerGet;
using Application.UseCases.Producer.ProducerUpdate;
using AutoMapper;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProducerController(
    IMediator mediator,
    IMapper mapper
) : ControllerBase
{
    [HttpGet("{id:guid:required}")]
    public async Task<IActionResult> GetProducerById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(mapper.Map<ProducerGetCommand>(id), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProducer([FromBody] ProducerCreateDTO dto, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(mapper.Map<ProducerCreateCommand>(dto), cancellationToken));
    }

    [HttpPut("{id:guid:required}")]
    //[Authorize(Roles = nameof(Roles.ProducerAdmin))]
    public async Task<IActionResult> UpdateProducer(Guid id, [FromBody] ProducerCreateDTO dto, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<ProducerUpdateCommand>((dto, id)), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid:required}")]
    [Authorize(Roles = nameof(Roles.ProducerAdmin))]
    public async Task<IActionResult> DeleteProducer(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<ProducerDeleteCommand>(id), cancellationToken);
        return Ok();
    }
}

