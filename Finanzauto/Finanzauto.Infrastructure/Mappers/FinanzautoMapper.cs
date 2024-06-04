using AutoMapper;
using Finanzauto.Domain.DTOs;
using Finanzauto.Domain.Entities;

namespace Finanzauto.Infrastructure.Mappers
{
    public class FinanzautoMapper : Profile
    {
        public FinanzautoMapper()
        {
            CreateMap<Brand, CreateBrandDto>().ReverseMap();
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Phase, PhaseDto>().ReverseMap();
            CreateMap<Phase, CreatePhaseDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<Vehicle, CreateVehicleDto>().ReverseMap();
            CreateMap<Vehicle, VehicleDto>().ReverseMap();
            CreateMap<Vehicle, DataVehicleDto>().ReverseMap();
            CreateMap<Vehicle, EditVehicleDto>().ReverseMap();
            CreateMap<Vehicle, InformationVehicleDto>().ReverseMap();
            CreateMap<VehiclePhoto, CreatePhotoDto>().ReverseMap();
            CreateMap<VehiclePhoto, PhotoDto>().ReverseMap();
            CreateMap<VehicleAudit, CreateAuditDto>().ReverseMap();
            CreateMap<Client, CreateClientDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
    
}
