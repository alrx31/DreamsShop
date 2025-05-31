using Application.DTO;
using Application.UseCases.Dreams.DreamCreate;
using Application.UseCases.Dreams.DreamDelete;
using Application.UseCases.Dreams.DreamGetAll;
using Application.UseCases.Dreams.DreamsGetOne;
using AutoMapper;
using Domain.Entity;

namespace Application.MappingProfiles;

public class DreamProfile : Profile
{
    public DreamProfile()
    {
        CreateMap<DreamCreateDto, DreamCreateCommand>()
            .ConstructUsing(dest => new DreamCreateCommand(dest));

        CreateMap<DreamCreateCommand, Dream>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Dto.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Dto.Description))
            .ForMember(dest => dest.ProducerId, opt => opt.MapFrom(src => src.Dto.ProducerId))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Dto.Rating));

        CreateMap<Guid, DreamGetOneCommand>()
            .ForMember(dest => dest.DreamId, opt => opt.MapFrom(src => src));

        CreateMap<(int, int), DreamGetAllCommand>()
            .ForMember(dest=>dest.StartIndex, opt=>opt.MapFrom(src=>src.Item1))
            .ForMember(dest=>dest.Count, opt=>opt.MapFrom(src=>src.Item2));
        
        CreateMap<Guid, DreamDeleteCommand>()
            .ForMember(dest=>dest.DreamId, opt=>opt.MapFrom(src=>src));
    }
}