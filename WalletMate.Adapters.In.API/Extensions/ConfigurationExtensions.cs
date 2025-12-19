using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WalletMate.Adapters.In.API.Extensions;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Configures Swagger documentation generation for the application, including
    /// custom OpenAPI settings, security definitions, and XML comments integration.
    /// </summary>
    /// <param name="builder">An instance of <see cref="WebApplicationBuilder"/> used to configure the application.</param>
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            
            options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");
            options.UseInlineDefinitionsForEnums();
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

    /// <summary>
    /// Configures JWT-based authentication for the application using settings defined in the configuration.
    /// </summary>
    /// <param name="builder">An instance of <see cref="WebApplicationBuilder"/> used to configure the application.</param>
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

    /// <summary>
    /// Configures authorization policies for the application, including setting
    /// the default policy to require authenticated users using the JWT Bearer
    /// authentication scheme.
    /// </summary>
    /// <param name="builder">An instance of <see cref="WebApplicationBuilder"/> used to configure the application's services.</param>
    public static void ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
    }
}