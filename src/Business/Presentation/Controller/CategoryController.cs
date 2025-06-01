using Application.DTO;
using Application.UseCases.Category.CategoryCreate;
using Application.UseCases.Category.CategoryGet;
using Application.UseCases.Category.CategoryGetAll;
using Application.UseCases.Category.CategoryRemove;
using Application.UseCases.Category.CategoryUpdate;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller;

[ApiController]
[Route("[controller]")]
public class CategoryController(
    IMediator mediator,
    IMapper mapper
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto model)
    {
        await mediator.Send(mapper.Map<CategoryAddCommand>(model));
        return Ok();
    }

    [HttpDelete("{categoryId:required:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid categoryId)
    {
        await mediator.Send(mapper.Map<CategoryRemoveCommand>(categoryId));
        return Ok();
    }

    [HttpGet("{categoryId:required:guid}")]
    public async Task<IActionResult> GetCategory(Guid categoryId)
    {
        return Ok(await mediator.Send(mapper.Map<CategoryGetCommand>(categoryId)));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok( await mediator.Send(new CategoryGetAllCommand()));
    }

    [HttpPut("{categoryId:required:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] CategoryUpdateDto model)
    {
        await mediator.Send(
            mapper.Map<CategoryUpdateCommand>( (categoryId, model) )
            );
        return Ok();
    }
}