using Application.DTO.Producer;
using Application.UseCases.Producer.ProducerCreate;
using Application.UseCases.Producer.ProducerDelete;
using Application.UseCases.Producer.ProducerGet;
using Application.UseCases.Producer.ProducerUpdate;
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
            .ConstructUsing(dest => new ProducerCreateCommand(dest));

        CreateMap<ProducerCreateDTO, Producer>();

        CreateMap<Guid, ProducerDeleteCommand>()
            .ConstructUsing(dest => new ProducerDeleteCommand(dest));

        CreateMap<(ProducerCreateDTO, Guid), ProducerUpdateCommand>()
            .ConstructUsing(dest => new ProducerUpdateCommand(dest.Item1, dest.Item2));
    }
}
