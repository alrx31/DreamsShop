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
    public async Task<IActionResult> RegisterProducerUser()
    {
        return Ok();
    }
}