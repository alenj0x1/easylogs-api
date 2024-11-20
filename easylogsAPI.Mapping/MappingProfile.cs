using AutoMapper;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Dto;

namespace easylogsAPI.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Userapp, UserAppMeDto>().ReverseMap();
        CreateMap<Userapp, UserAppDefaultDto>().ReverseMap();
        CreateMap<Userapppermission, UserAppPermissionDto>().ReverseMap();
        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<Sessiontype, SessionTypeDto>().ReverseMap();
        CreateMap<Log, LogDto>().ReverseMap();
        CreateMap<Logtype, LogTypeDto>().ReverseMap();
        CreateMap<Tokenaccess, TokenAccessDto>().ReverseMap();
    }
}