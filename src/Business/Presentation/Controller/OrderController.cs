using Application.DTO;
using Application.UseCases.Order.CreateOrder;
using Application.UseCases.Order.OrderGetAllByUser;
using Application.UseCases.Order.OrderGetOne;
using AutoMapper;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(
        IMediator mediator,
        IMapper mapper
    ) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = nameof(Roles.Consumer))]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto model)
        {
            return Ok(await mediator.Send(mapper.Map<OrderCreateCommand>(model)));
        }

        [HttpGet("{id:guid:required}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            return Ok(await mediator.Send(mapper.Map<OrderGetOneCommand>(id)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserOrders()
        {
            return Ok(await mediator.Send(new OrderGetAllByUserCommand()));
        }
    }
}
