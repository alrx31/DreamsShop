using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.MappingProfiles;

public class UserMapperProfile:Profile
{
    public UserMapperProfile()
    {
        CreateMap<RegisterUserDTO, User>()
            .ForMember(u => u.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(u => u.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(u => u.Password, opt => opt.MapFrom(src => src.Password));

        CreateMap<User, ResponseUser>();
    }
    
}