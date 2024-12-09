using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.ControllersMinimal;
using OnlineShop.Services;
using OnlineShop.Services.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Setup logging with Serilog
builder.Services.SerilogLoggerService(builder.Configuration);

// Bootstrap logging
/* builder.Host.SerilogLoggerGlobalHost(); */

Log.Information("App is running");

// Add services to the container.
// Add authentication and authorization
builder.Services.AddJwtAuthenticationService(builder.Configuration);
builder.Services.AddAuthorization();

// Add swagger
builder.Services.AddSwaggerGenService();

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

// NOTE: Minimal apis register here
app.MapProductController();
app.MapUserController();
app.MapCartController();

app.Run();
