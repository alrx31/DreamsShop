using Application.DTO;
using Application.DTO.Order;
using Application.UseCases.Order.CreateOrder;
using Application.UseCases.Order.OrderGetOne;
using AutoMapper;
using Domain.Entity;

namespace Application.MappingProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderCreateDto, OrderCreateCommand>()
            .ConstructUsing(src => new OrderCreateCommand(src));

        CreateMap<Guid, OrderGetOneCommand>()
            .ConstructUsing(src => new OrderGetOneCommand(src));

        CreateMap<OrderDream, OrderDreamDto>()
            .ForMember(dest => dest.DreamId, opt => opt.MapFrom(src => src.DreamId));

        CreateMap<Order, OrderResponseDto>();
    }
}
