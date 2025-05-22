using Application.DTO;
using AutoMapper;
using Domain.Entity;

namespace Application.MappingProfiles;

public class ConsumerUserMapperProfile : Profile
{
    public ConsumerUserMapperProfile()
    {
        CreateMap<ConsumerUserRegisterDto,ConsumerUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());;
    }
    
}