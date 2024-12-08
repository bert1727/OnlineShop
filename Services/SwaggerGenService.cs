using System.Reflection;
using Microsoft.OpenApi.Models;

namespace OnlineShop.Services;

public static class SwaggerGenService
{
    public static void AddSwaggerGenService(this IServiceCollection services)
    {
        /* services.AddSwaggerGen(); */
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(static options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OnlineShop",
                    Description = "An ASP.NET Core Web API for managing online shop",
                    /* TermsOfService = new Uri("https://example.com/terms"), */
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license"),
                    },
                }
            );
            options.SwaggerDoc("v2", new OpenApiInfo { Title = "My API - V2", Version = "v2" });

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token",
                }
            );

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                }
            );

            string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}
