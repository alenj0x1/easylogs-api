using AutoMapper;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Dto;

namespace easylogsAPI.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Userapp, UserDto>().ReverseMap();
        CreateMap<Logtype, LogTypeDto>().ReverseMap();
        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<Log, LogDto>().ReverseMap();
    }
}