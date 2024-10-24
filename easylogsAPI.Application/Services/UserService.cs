using System.Reflection;
using AutoMapper;
using easylogsAPI.Application.Helpers;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;
using easylogsAPI.Shared;
using easylogsAPI.Shared.Consts;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Application.Services;

public class UserService(IUserRepository userRepository, IMapper mapper, ILogger<IUserService> logger) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IUserService> _logger = logger;
    private readonly ServiceData _serviceData = new ServiceData();
    
    public async Task<BaseResponse<UserDto>> Create(CreateUserRequest request)
    {
        try
        {
            var crt = await _userRepository.Create(new Userapp()
            {
                Username = request.Username,
                Email = request.Email,
                Password = Hasher.HashPassword(request.Password)
            });
            var mp = _mapper.Map<UserDto>(crt);

            return _serviceData.CreateResponse(mp, ResponseConsts.UserCreated, 201);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<UserDto> Get(Guid userappId)
    {
        try
        {
            var dt = _userRepository.Get(userappId);
            var mp = _mapper.Map<UserDto>(dt);

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<List<UserDto>> Get()
    {
        try
        {
            var gt = _userRepository.Get();
            var mp = _mapper.Map<List<UserDto>>(gt);

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<BaseResponse<UserDto>> Update(UpdateUserRequest request, Guid userappId)
    {
        try
        {
            var gt = _userRepository.Get(userappId) ?? throw new Exception(ResponseConsts.UserNotFound);
            
            gt.Username = request.Username ?? gt.Username;
            gt.Password = request.Password is not null ? Hasher.HashPassword(request.Password) : gt.Password;
            gt.Email = request.Email ?? gt.Email;
            var upd = await _userRepository.Update(gt);
            var mp = _mapper.Map<UserDto>(upd);

            return _serviceData.CreateResponse(mp, ResponseConsts.UserUpdated, 204);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<BaseResponse<bool>> Delete(Guid userappId)
    {
        try
        {
            var gt = _userRepository.Get(userappId) ?? throw new Exception(ResponseConsts.UserNotFound);
            var del = await _userRepository.Delete(gt);
            
            return _serviceData.CreateResponse(del, ResponseConsts.UserDeleted);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}