using Application.DTO.ProducerUser;
using Application.UseCases.ProducerUserAuth.ProducerUserLogin;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using AutoMapper;
using Domain.Entity;
using Domain.IServices;

namespace Application.MappingProfiles;

public class ProducerUserMapperProfile : Profile
{
    public ProducerUserMapperProfile()
    {
        CreateMap<ProducerUserRegisterDto, ProducerUserRegisterCommand>()
            .ConstructUsing(ctor => new ProducerUserRegisterCommand(ctor));

        CreateMap<ProducerUserRegisterCommand, ProducerUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest =>dest.Name, opt=>opt.MapFrom(x=>x.Dto.Name))
            .ForMember(dest =>dest.Email, opt=>opt.MapFrom(x=>x.Dto.Email))
            .ForMember(dest => dest.Password,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ((IPasswordManager)context.Items["PasswordHasher"]).HashPassword(src.Dto.Password)));
        
        CreateMap<ProducerUserLoginDto, ProducerUserLoginCommand>()
            .ConstructUsing(ctor => new ProducerUserLoginCommand(ctor));
    }
}