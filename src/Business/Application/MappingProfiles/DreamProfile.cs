using Application.DTO;
using Application.UseCases.Dreams.DreamCreate;
using Application.UseCases.Dreams.DreamDelete;
using Application.UseCases.Dreams.DreamGetAll;
using Application.UseCases.Dreams.DreamsGetOne;
using Application.UseCases.Dreams.DreamUpdate;
using AutoMapper;
using Domain.Entity;
using Domain.Model;

namespace Application.MappingProfiles;

public class DreamProfile : Profile
{
    public DreamProfile()
    {
        CreateMap<DreamCreateDto, DreamCreateCommand>()
            .ConvertUsing((src,_,_) => new DreamCreateCommand(
                Title: src.Title,
                Description: src.Description,
                ProducerId: src.ProducerId,
                Rating: src.Rating,
                Image: src.Image == null 
                    ? null 
                    : new FileModel
                    {
                        FileName = src.Image.FileName,
                        ContentType = src.Image.ContentType,
                        Content = src.Image.OpenReadStream()
                    }
            ));

        CreateMap<DreamCreateCommand, Dream>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ProducerId, opt => opt.MapFrom(src => src.ProducerId))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

        CreateMap<Guid, DreamGetOneCommand>()
            .ForMember(dest => dest.DreamId, opt => opt.MapFrom(src => src));

        CreateMap<(int, int), DreamGetAllCommand>()
            .ForMember(dest=>dest.StartIndex, opt=>opt.MapFrom(src=>src.Item1))
            .ForMember(dest=>dest.Count, opt=>opt.MapFrom(src=>src.Item2));
        
        CreateMap<Guid, DreamDeleteCommand>()
            .ForMember(dest=>dest.DreamId, opt=>opt.MapFrom(src=>src));

        CreateMap<(Guid, DreamUpdateDto), DreamUpdateCommand>()
            .ForMember(dest => dest.DreamId, opt => opt.MapFrom(src => src.Item1))
            .ForMember(dest => dest.Dto, opt => opt.MapFrom(src => src.Item2));
    }
}