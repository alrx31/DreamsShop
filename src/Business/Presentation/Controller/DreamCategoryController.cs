using Application.UseCases.DreamCategory.DreamCategoryAdd;
using Application.UseCases.DreamCategory.DreamCategoryDelete;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller;

[ApiController]
[Route("api/[controller]")]
public class DreamCategoryController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddDreamCategory(Guid dreamId, Guid categoryId)
    {
        await mediator.Send(
            mapper.Map<DreamCategoryAddCommand>((dreamId, categoryId)));
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDreamCategory(Guid dreamId, Guid categoryId)
    {
        await mediator.Send(mapper.Map<DreamCategoryDeleteCommand>( (dreamId, categoryId) ));
        return Ok();
    }
}