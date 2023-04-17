using AutoMapper;
using EuvicIntern.Entities;
using EuvicIntern.Models;

namespace EuvicIntern
{
    public class EuvicMappProfile : Profile
    {
        public EuvicMappProfile()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<User, ReturnUserDto>()
                .ForMember(r => r.Role, u => u.MapFrom(x => x.Role.Name));
        }
    }
}
