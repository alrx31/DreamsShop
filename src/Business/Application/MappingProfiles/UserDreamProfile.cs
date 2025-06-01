using Application.UseCases.UserDream.UserDreamAdd;
using Application.UseCases.UserDream.UserDreamDelete;
using AutoMapper;
using Domain.Entity;

namespace Application.MappingProfiles;

public class UserDreamProfile : Profile
{
    public UserDreamProfile()
    {
        CreateMap<Guid, UserDreamAddCommand>()
            .ConstructUsing(ctor => new UserDreamAddCommand(ctor));

        CreateMap<(Guid, Guid), UserDream>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Item1.ToString()))
            .ForMember(dest => dest.DreamId, opt => opt.MapFrom(src => src.Item2.ToString()));

        CreateMap<Guid, UserDreamDeleteCommand>()
            .ConstructUsing(ctor => new UserDreamDeleteCommand(ctor));
    }
}