using Application.DTO;
using Application.UseCases.CommandHandler;
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
    }
}