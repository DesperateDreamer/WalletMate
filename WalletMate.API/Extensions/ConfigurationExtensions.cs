using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WalletMate.BLL.Domain;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared;
using WalletMate.BLL.Shared.Abstract;
using WalletMate.DAL.Context;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;

namespace WalletMate.API.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureInAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<IPasswordService, PasswordService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
    }
    
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "WalletMate API",
                Version = "v1",
            });
        });
    }

    public static void ConfigureDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
        
        builder.Services.AddDbContext<IDataContext, DataContext>((_, options) =>
        {
            options
                .UseNpgsql(connectionString, x => x.MigrationsAssembly("WalletMate.DAL.Migrations"));
        });
    }
}