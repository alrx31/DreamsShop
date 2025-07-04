using Application.DTO;
using Application.UseCases.Order.CreateOrder;
using Application.UseCases.Order.OrderGetOne;
using AutoMapper;

namespace Application.MappingProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderCreateDto, OrderCreateCommand>()
            .ConstructUsing(src => new OrderCreateCommand(src));

        CreateMap<Guid, OrderGetOneCommand>()
            .ConstructUsing(src => new OrderGetOneCommand(src));
    }
}
