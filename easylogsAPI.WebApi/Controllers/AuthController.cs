﻿using System.Reflection;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.Auth;
using easylogsAPI.Models.Responses;
using easylogsAPI.Models.Responses.Auth;
using easylogsAPI.WebApi.Attributes;
using easylogsAPI.WebApi.Interfaces.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService, ILogger<IAuthController> logger) : ControllerBase, IAuthController
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<IAuthController> _logger = logger;

    [HttpPost("accessToken/create")]
    [Permission("CREATE_ACCESS_TOKEN")]
    [Authorize]
    public async Task<BaseResponse<LoginAuthResponse>> CreateAccessToken(CreateAccessTokenAuthRequest request)
    {
        try
        {
            return await _authService.CreateAccessToken(HttpContext, request);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpGet("validateToken")]
    [Authorize]
    public BaseResponse<string> ValidateToken()
    {
        try
        {
            return _authService.ValidateToken();
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpPost("accessToken")]
    [Authorize]
    public BaseResponse<List<TokenAccessDto>> GetTokenAccesses(BaseRequest request)
    {
        try
        {
            return _authService.GetTokenAccesses(request);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpDelete("accessToken/delete/{id:int}")]
    [Permission("REMOVE_ACCESS_TOKEN")]
    [Authorize]
    public async Task<BaseResponse<bool>> RemoveAccessToken(int id)
    {
        try
        {
            return await _authService.RemoveAccessToken(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpPost("login")]
    public async Task<BaseResponse<LoginAuthResponse>> Login([FromBody] LoginAuthRequest request)
    {
        try
        {
            return await _authService.Login(HttpContext, request);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpPost("renewAccess")]
    public async Task<BaseResponse<LoginAuthResponse>> RenewAccess([FromBody] RenewAccessAuthRequest request)
    {
        try
        {
            return await _authService.RenewAccess(HttpContext, request);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}