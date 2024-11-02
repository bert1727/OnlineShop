using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Controllers;
using OnlineShop.Services;
using OnlineShop.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add db context
string? connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
builder.Services.AddDbContext<ProductDbContext>(opt => opt.UseSqlite(connectionString));

// My services
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICartService, CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* app.UseExceptionHandler(errorApp => */
/* { */
/*     errorApp.Run(async context => */
/*     { */
/*         var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error; */
/**/
/*         if (exception is BadHttpRequestException) */
/*         { */
/*             context.Response.StatusCode = StatusCodes.Status400BadRequest; */
/*             await context.Response.WriteAsJsonAsync( */
/*                 new { error = "Invalid JSON format", details = exception.Message } */
/*             ); */
/*         } */
/*         else */
/*         { */
/*             context.Response.StatusCode = StatusCodes.Status500InternalServerError; */
/*             await context.Response.WriteAsJsonAsync( */
/*                 new { error = "An unexpected error occurred", details = exception?.Message } */
/*             ); */
/*         } */
/*     }); */
/* }); */

// NOTE: Minimal apis was registered here
app.MapProductController();
app.MapUserController();
app.MapCartController();

app.Run();
