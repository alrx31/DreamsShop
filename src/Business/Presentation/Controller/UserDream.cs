using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserDream(IMediator mediator, IMapper mapper) : ControllerBase
{
}