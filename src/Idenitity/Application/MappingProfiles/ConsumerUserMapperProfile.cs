using Application.DTO;
using AutoMapper;
using Domain.Entity;
using Domain.IServices;

namespace Application.MappingProfiles;

public class ConsumerUserMapperProfile : Profile
{
    public ConsumerUserMapperProfile()
    {
        CreateMap<ConsumerUserRegisterDto, ConsumerUser>()
            .ForMember
            (
                dest => dest.Id,
                opt => opt.Ignore()
            )
            .ForMember(dest => dest.Password,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ((IPasswordManager)context.Items["PasswordHasher"]).HashPassword(src.Password)));

        CreateMap<ConsumerUserRegisterDto, ConsumerUserLoginDto>()
            .ForMember(
                x => x.Password,
                opt => opt.MapFrom(x => x.Password))
            .ForMember(
                x => x.Email,
                opt => opt.MapFrom(x => x.Email));
    }
}