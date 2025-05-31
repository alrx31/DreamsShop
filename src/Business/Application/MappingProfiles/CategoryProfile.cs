using Application.DTO;
using Application.UseCases.Category.CategoryAdd;
using Application.UseCases.Category.CategoryGet;
using Application.UseCases.Category.CategoryRemove;
using Application.UseCases.Category.CategoryUpdate;
using AutoMapper;
using Domain.Entity;

namespace Application.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryCreateDto, CategoryAddCommand>()
            .ConstructUsing(ctor => new CategoryAddCommand(ctor));

        CreateMap<CategoryAddCommand, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(x => x.Dto.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Dto.Description));

        CreateMap<Guid, CategoryRemoveCommand>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(x => x));
        
        CreateMap<Guid, CategoryGetCommand>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(x => x));

        CreateMap<(Guid, CategoryUpdateDto), CategoryUpdateCommand>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(x => x.Item1))
            .ForMember(dest => dest.Dto, opt => opt.MapFrom(x => x.Item2));
    }
}