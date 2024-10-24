using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Shared;
using easylogsAPI.Shared.Consts;
using Microsoft.Extensions.Configuration;

namespace easylogsAPI.Application.Helpers;

public static class Verify
{
    public static async Task<bool> FirstUserCreation(IUserRepository userRepository, IConfiguration configuration)
    {
        try
        {
            if (userRepository.Get().ToList().Count > 0) return false;

            var password = configuration["FirstUser:Password"] ?? throw new Exception(ResponseConsts.MissingConfigurationFirstUserPassword);
            await userRepository.Create(new Userapp()
            {
                Username = configuration["FirstUser:Username"] ?? throw new Exception(ResponseConsts.MissingConfigurationFirstUserUsername),
                Password = Hasher.HashPassword(password),
                Email = configuration["FirstUser:Email"] ?? throw new Exception(ResponseConsts.MissingConfigurationFirstUserEmail),
            });
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}