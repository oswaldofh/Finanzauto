using AutoMapper;
using Finanzauto.Domain.DTOs;
using Finanzauto.Domain.Entities;

namespace Finanzauto.Infrastructure.Mappers
{
    public class FinanzautoMapper : Profile
    {
        public FinanzautoMapper()
        {
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CreateCityDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
        }
    }
}
