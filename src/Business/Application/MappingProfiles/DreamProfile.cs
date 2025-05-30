using Application.DTO;
using Application.UseCases.DreamCreate;
using Application.UseCases.DreamsGetOne;
using AutoMapper;
using Domain.Entity;

namespace Application.MappingProfiles;

public class DreamProfile : Profile
{
    public DreamProfile()
    {
        CreateMap<DreamCreateDto, DreamCreateCommand>()
            .ConstructUsing(x => new DreamCreateCommand(x));

        CreateMap<DreamCreateCommand, Dream>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Guid, DreamGetOneCommand>()
            .ForMember(dest => dest.DreamId, opt => opt.MapFrom(src => src));
    }
}