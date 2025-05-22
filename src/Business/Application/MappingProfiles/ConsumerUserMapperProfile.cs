using Application.DTO;
using Application.UseCases.Commands;
using AutoMapper;

namespace Application.MappingProfiles;

public class ConsumerUserMapperProfile : Profile
{
    public ConsumerUserMapperProfile()
    {
        CreateMap<ConsumerUserRegisterDto, ConsumerUserRegisterCommand>();
    }
    
}