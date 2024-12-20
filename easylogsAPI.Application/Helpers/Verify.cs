﻿using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Shared;
using easylogsAPI.Shared.Consts;
using Microsoft.Extensions.Configuration;

namespace easylogsAPI.Application.Helpers;

public static class Verify
{
    public static async Task<bool> FirstUserCreation(IUserRepository userRepository, IUserPermissionRepository userPermissionRepository, IConfiguration configuration)
    {
        try
        {
            if (userRepository.Get().ToList().Count > 0) return false;

            var password = configuration["FirstUser:Password"] ?? throw new Exception(ResponseConsts.ConfigurationMissingFirstUserPassword);
            
            var usr = await userRepository.Create(new Userapp()
            {
                Username = configuration["FirstUser:Username"] ?? throw new Exception(ResponseConsts.ConfigurationMissingFirstUserUsername),
                Password = Hasher.HashPassword(password),
                Email = configuration["FirstUser:Email"] ?? throw new Exception(ResponseConsts.ConfigurationMissingFirstUserEmail),
            });

            await userPermissionRepository.Create(new Userapppermission
            {
                PermissionId = PermissionConsts.Administrator,
                UserAppId = usr.UserAppId,
                GivenAt = DateTime.UtcNow,
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