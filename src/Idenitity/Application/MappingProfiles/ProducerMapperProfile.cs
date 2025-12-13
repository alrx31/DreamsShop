using Application.DTO.Producer;
using Application.DTO.ProducerUser;
using Application.UseCases.Producer.ProducerCreate;
using Application.UseCases.Producer.ProducerDelete;
using Application.UseCases.Producer.ProducerGet;
using Application.UseCases.Producer.ProducerUpdate;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using AutoMapper;
using Domain.Entity;

namespace Application.MappingProfiles;

public class ProducerMapperProfile : Profile
{
    public ProducerMapperProfile()
    {
        CreateMap<Guid, ProducerGetCommand>()
            .ConstructUsing(dest => new ProducerGetCommand(dest));

        CreateMap<ProducerCreateDTO, ProducerCreateCommand>()
            .ConstructUsing(src => new ProducerCreateCommand(
                src,
                new ProducerUserRegisterCommand(src.ProducerUser!)));

        CreateMap<ProducerCreateDTO, Producer>();

        CreateMap<Guid, ProducerDeleteCommand>()
            .ConstructUsing(dest => new ProducerDeleteCommand(dest));

        CreateMap<(ProducerCreateDTO, Guid), ProducerUpdateCommand>()
            .ConstructUsing(dest => new ProducerUpdateCommand(dest.Item1, dest.Item2));
    }
}
