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
        }
    }
}
