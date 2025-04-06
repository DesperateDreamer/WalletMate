using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WalletMate.BLL.Domain;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared;
using WalletMate.BLL.Shared.Abstract;
using WalletMate.DAL.Context;
using WalletMate.DAL.Context.Abstract;
using WalletMate.DAL.Entities;
using WalletMate.External.Monobank;

namespace WalletMate.API.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureInAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<IPasswordService, PasswordService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
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
            options.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "BearerAuth",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "BearerAuth",
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });
    }

    public static void ConfigureJwtAuthentication(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");

        builder.Services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? string.Empty))
                };
            });
    }
    
    public static void ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
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

    public static void ConfigureMonobankClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient<IMonobankClient, MonobankClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.monobank.ua");
            client.DefaultRequestHeaders.Add("User-Agent", "WalletMate-Client");
        });
    }
}