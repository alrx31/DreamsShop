using Application.UseCases.DreamCategory.DreamCategoryAdd;
using Application.UseCases.DreamCategory.DreamCategoryDelete;
using AutoMapper;
using Domain.Entity;

namespace Application.MappingProfiles;

public class DreamCategoryProfile : Profile
{
    public DreamCategoryProfile()
    {
        CreateMap<(Guid, Guid), DreamCategoryAddCommand>()
            .ConstructUsing(ctor=>new DreamCategoryAddCommand(ctor.Item1, ctor.Item2));

        CreateMap<DreamCategoryAddCommand, DreamCategory>();

        CreateMap<(Guid, Guid), DreamCategoryDeleteCommand>()
            .ConstructUsing(ctor => new DreamCategoryDeleteCommand(ctor.Item1, ctor.Item2));
    }
}