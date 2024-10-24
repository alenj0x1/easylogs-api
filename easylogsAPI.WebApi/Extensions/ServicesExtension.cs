using System.Text;
using easylogsAPI.Application.Helpers;
using easylogsAPI.Application.Interfaces.Helpers;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Application.Services;
using easylogsAPI.Domain.Context;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Domain.Repositories;
using easylogsAPI.Mapping;
using easylogsAPI.Shared.Consts;
using easylogsAPI.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Log = Serilog.Log;

namespace easylogsAPI.WebApi.Extensions;

public static class ServicesExtension
{
    public static async Task AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddSerilog();
        
        // Authentication and authorization
        services.AddAuthentication(builder =>
        {
            builder.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            builder.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(builder =>
        {
            builder.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"] ?? throw new Exception(ResponseConsts.MissingConfigurationJwtIssuer),
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"] ?? throw new Exception(ResponseConsts.MissingConfigurationJwtAudience),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"] ?? throw new Exception(ResponseConsts.MissingConfigurationJwtSecretKey))),
                ValidateLifetime = false // <- by default is true
            };
            builder.Events = new JwtBearerEvents()
            {
                // Customized events
                OnTokenValidated = async context =>
                {
                    var securityToken = context.Request.Headers.Authorization.ToString()[7..];
                    if (string.IsNullOrEmpty(securityToken))
                    {
                        context.Fail(ResponseConsts.TokenNotFound);
                        return;
                    }
                        
                    var tokenRepository = context.HttpContext.RequestServices.GetRequiredService<ITokenRepository>();
                   
                    var tkAccess = tokenRepository.GetTokenAccess(securityToken); // <- search on database
                    
                    if (tkAccess is null)
                    {
                        context.Fail(ResponseConsts.TokenNotFound);
                        return;
                    }
                    
                    var tkRefresh = tokenRepository.GetTokenRefresh(tkAccess.UserAppId);; // <- define token refresh, for delete cases
                    
                    if (DateTime.Now.CompareTo(tkAccess.Expiration) >= 0) // <- check expiration
                    {
                        if (tkRefresh is not null)  await tokenRepository.DeleteTokenRefresh(tkRefresh);
                        context.Fail(ResponseConsts.TokenExpired);
                    }

                    if (tkAccess.Ip != context.HttpContext.Connection.RemoteIpAddress?.ToString()) // <- check ip address
                    {
                        if (tkRefresh is not null) await tokenRepository.DeleteTokenRefresh(tkRefresh);
                        context.Fail(ResponseConsts.TokenNotFound);
                    }
                },
                OnChallenge = async context =>
                {
                    if (!string.IsNullOrWhiteSpace(context.AuthenticateFailure?.Message))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync(context.AuthenticateFailure.Message);
                    }
                }
            };
        });

        services.AddAuthorization();
        
        // Database
        services.AddDbContext<EasylogsDbContext>(builder =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("postgres") ?? throw new Exception(ResponseConsts.MissingConfigurationPostgresConnectionString));
        });

        // Automapper
        services.AddAutoMapper(typeof(MappingProfile));
        
        // Middlewares
        services.AddScoped(typeof(ErrorHandlerMiddleware));
        
        // Services
        services.AddScoped<IAppService, AppService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        
        // Repositories
        services.AddScoped<IAppRepository, AppRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        
        // Helpers
        services.AddScoped<IToken, Token>();
        
        // Controllers
        services.AddControllers();
        
        // First user creation
        var userRepository = services.BuildServiceProvider().GetRequiredService<IUserRepository>();
        if (await Verify.FirstUserCreation(userRepository, configuration))
        {
            Log.Information(ResponseConsts.UserFirstCreated);
        }
    }
}