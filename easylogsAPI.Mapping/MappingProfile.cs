using AutoMapper;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Dto;

namespace easylogsAPI.Mapping;

public class MappingProfile : Profile
{
    private readonly IAppRepository _appRepository;
    
    public MappingProfile()
    {
        CreateMap<Userapp, UserAppDto>().ReverseMap();
        CreateMap<Userapppermission, UserAppPermissionDto>().ReverseMap();
        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<Sessiontype, SessionTypeDto>().ReverseMap();
        CreateMap<Log, LogDto>().ReverseMap();
        CreateMap<Logtype, LogTypeDto>().ReverseMap();
    }
}