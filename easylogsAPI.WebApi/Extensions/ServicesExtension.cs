using System.Globalization;
using System.Text;
using easylogsAPI.Application.Helpers;
using easylogsAPI.Application.Interfaces.Helpers;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Application.Services;
using easylogsAPI.Domain.Context;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Domain.Repositories;
using easylogsAPI.Mapping;
using easylogsAPI.Shared.Consts;
using easylogsAPI.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        
        // Integrate Swagger security feature
        services.AddSwaggerGen(options =>
        {
           options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
           {
               Name = "Authorization",
               In = ParameterLocation.Header,
               Type = SecuritySchemeType.Http,
               Scheme = "Bearer"
           });
           
           options.AddSecurityRequirement(new OpenApiSecurityRequirement
           {
               {
                   new OpenApiSecurityScheme
                   {
                       Reference = new OpenApiReference
                       {
                           Type = ReferenceType.SecurityScheme,
                           Id = "Bearer"
                       }
                   },
                   Array.Empty<string>()
               }
           });
        });
        
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
                ValidIssuer = configuration["Jwt:Issuer"] ?? throw new Exception(ResponseConsts.ConfigurationMissingJwtIssuer),
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"] ?? throw new Exception(ResponseConsts.ConfigurationMissingJwtAudience),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"] ?? throw new Exception(ResponseConsts.ConfigurationMissingJwtSecretKey))),
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

                    if (tkAccess.IsApiKey && !ApiKeyConsts.AllowedPaths.Contains(context.HttpContext.Request.Path.Value))
                    {
                        context.Fail(ResponseConsts.TokenNotFound);
                    }
                    
                    var tkRefresh = tokenRepository.GetTokenRefresh(tkAccess.UserAppId); // <- define token refresh, for delete cases
                    
                    if (DateTime.UtcNow.CompareTo(tkAccess.Expiration) >= 0) // <- check expiration
                    {
                        // if (tkRefresh is not null)  await tokenRepository.DeleteTokenRefresh(tkRefresh);
                        context.Fail(ResponseConsts.TokenExpired);
                    }

                    if (tkAccess.Ip == "ignore_validation") return;
                    if (tkAccess.Ip != context.HttpContext.Connection.RemoteIpAddress?.ToString()) // <- check ip address
                    {
                        if (tkRefresh is not null) await tokenRepository.DeleteTokenRefresh(tkRefresh);
                        context.Fail(ResponseConsts.TokenNotFound);
                    }
                },
                OnChallenge = context => throw (context.AuthenticateFailure?.InnerException switch
                {
                    SecurityTokenMalformedException => new Exception(ResponseConsts.TokenNotFound),
                    _ => new Exception(context.AuthenticateFailure?.Message)
                })
            };
        });

        services.AddAuthorization();
        
        // Database
        services.AddDbContext<EasyLogsDbContext>(builder =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("postgres") ?? throw new Exception(ResponseConsts.ConfigurationMissingPostgresConnectionString));
        });

        // Automapper
        services.AddAutoMapper(typeof(MappingProfile));
        
        // Middlewares
        services.AddScoped(typeof(ErrorHandlerMiddleware));
        services.AddScoped(typeof(ApiKeyMiddleware));
        services.AddScoped(typeof(PermissionMiddleware));
        
        // Services
        services.AddScoped<IAppService, AppService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILogService, LogService>();
        
        // Repositories
        services.AddScoped<IAppRepository, AppRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        
        // Helpers
        services.AddScoped<IToken, Token>();

        // Controllers
        services.AddControllers();
        
        // Localization
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        var supportedCultures = new List<CultureInfo>
        {
            new ("en-US"),
            new ("es-ES"),
            new ("ko-KR")
        };
        
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(configuration["DefaultLocalization"] ?? "en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        
        // First user creation
        var userRepository = services.BuildServiceProvider().GetRequiredService<IUserRepository>();
        var userPermissionRepository = services.BuildServiceProvider().GetRequiredService<IUserPermissionRepository>();
        if (await Verify.FirstUserCreation(userRepository, userPermissionRepository, configuration))
        {
            Log.Information(ResponseConsts.UserFirstCreated);
        }
    }
}