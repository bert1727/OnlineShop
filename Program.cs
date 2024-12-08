using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineShop.Context;
using OnlineShop.ControllersMinimal;
using OnlineShop.Models.DTOs;
using Onlineshop.Models.Enums;
using OnlineShop.Services;
using OnlineShop.Services.Interfaces;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Templates;
using Serilog.Templates.Themes;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateBootstrapLogger();

/* builder.Services.AddSerilog( */
/*     (services, lc) => */
/*         lc */
/*             .ReadFrom.Configuration(builder.Configuration) */
/*             .ReadFrom.Services(services) */
/*             .Enrich.FromLogContext() */
/*             .WriteTo.Console( */
/*                 new ExpressionTemplate( */
/*                     // Include trace and span ids when present. */
/*                     "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}", */
/*                     theme: TemplateTheme.Code */
/*                 ) */
/*             ) */
/* ); */
/**/
/* builder.Services.AddSerilog(); */

/* Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger(); */
/* builder.Services.AddSerilog( */
/*     (services, lc) => */
/*         lc */
/*             .ReadFrom.Configuration(config) */
/*             .ReadFrom.Services(services) */
/*             .Enrich.FromLogContext() */
/*             .WriteTo.Console() */
/* new ExpressionTemplate( */
// Include trace and span ids when present.
/* "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}", */
/* theme: TemplateTheme.Code */
/* ); */

builder.Host.UseSerilog(
    (context, config) =>
    {
        /* config.ReadFrom.Configuration(context.Configuration); */
        config
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                new JsonFormatter(),
                "./Logs/logs.json",
                rollingInterval: RollingInterval.Day
            );
    }
);

/* builder.Services.AddSerilog(); */
Log.Error("Приложение запустилось!!!");

/* Log.Logger = new LoggerConfiguration() */
/*     .MinimumLevel.Information() */
/*     .WriteTo.Console() */
/*     .WriteTo.File( */
/*         new CompactJsonFormatter(), */
/*         "./Logs/logs.json", */
/*         rollingInterval: RollingInterval.Day */
/*     ) */
/*     .CreateLogger(); */

Log.Information(
    "user is {@user}",
    new UserDto
    {
        Name = "213",
        Id = 1,
        Role = Role.Admin,
    }
);

// Adding authentication and authorization
// FIXME: put in a separate file
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)
            ),
        };
    });
builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // TODO: change this and figure out how to use it properly
    // FIXME: put in a separate file
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
            Description = "Введите токен в формате: Bearer <ваш-токен>",
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

// Add db context
string? connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
builder.Services.AddDbContext<OnlineShopDbContext>(opt => opt.UseSqlite(connectionString));

// My services
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICartService, CartService>();

// Add default controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// NOTE: Minimal apis was registered here
app.MapProductController();
app.MapUserController();
app.MapCartController();

app.Run();
