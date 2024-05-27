using AutoMapper;
using MolyCoreWeb.Models.DBEntitiy;
using MolyCoreWeb.Models.DTOs;

namespace MolyCoreWeb.Mappings
{
    // Profile class for AutoMapper
    public class MappingProfile : Profile
    {
        //instalation of AutoMapper
        public MappingProfile()
        {
            //creating mappings  is  for creating relatonship between two objects
            CreateMap<User, UserDto>();
           //     .ForMember() // Add property mappings here  
            CreateMap<UserDto, User>();
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UserProfileDto, UserProfile>();
        }
    }
}
