using easylogsAPI.Domain.Context;
using Microsoft.EntityFrameworkCore;

namespace easylogsAPI.WebApi.Extensions;

public static class ServicesExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<EasylogsDbContext>(builder =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("postgres") ?? throw new Exception("postgres database connection string missing"));
        });
    }
}