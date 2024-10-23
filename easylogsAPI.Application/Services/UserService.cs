using AutoMapper;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Shared;

namespace easylogsAPI.Application.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<UserDto> Create(CreateUserRequest request)
    {
        try
        {
            var crt = await _userRepository.Create(new Userapp()
            {
                Username = request.Username,
                Email = request.Email,
                Password = Hasher.HashPassword(request.Password)
            });

            return _mapper.Map<UserDto>(crt);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public UserDto? Get(Guid userappId)
    {
        try
        {
            var dt = _userRepository.Get(userappId);
            return _mapper.Map<UserDto>(dt);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<UserDto> Get()
    {
        try
        {
            var gt = _userRepository.Get();
            return _mapper.Map<List<UserDto>>(gt);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserDto> Update(UpdateUserRequest request)
    {
        try
        {
            var gt = _userRepository.Get(request.UserId) ?? throw new Exception("user not found");
            
            gt.Username = request.Username ?? gt.Username;
            gt.Password = request.Password is not null ? Hasher.HashPassword(request.Password) : gt.Password;
            gt.Email = request.Email ?? gt.Email;
            var upd = await _userRepository.Update(gt);
            
            return _mapper.Map<UserDto>(upd);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var gt = _userRepository.Get(id) ?? throw new Exception("user not found");
            
            var del = await _userRepository.Delete(gt);
            return del;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}