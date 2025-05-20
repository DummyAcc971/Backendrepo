using MyStockSymbolApi.Repositories;
using MyStockSymbolApi.Repositories.Interfaces;
using MyStockSymbolApi.Services;
using MyStockSymbolApi.Services.Interfaces;
using MyStockSymbolApi.Data;
using MyStockSymbolApi.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

// Add session services
builder.Services.AddDistributedMemoryCache(); // Required for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the cookie accessible only to the server
    options.Cookie.IsEssential = true; // Ensure the cookie is essential
});

// Register repository and services for dependency injection
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddSingleton<IStockSymbolRepository, StockSymbolRepository>();
builder.Services.AddSingleton<IStockSymbolService, StockSymbolService>();
builder.Services.AddSingleton<ITickerService, TickerService>();
builder.Services.AddScoped<UserService>(); // Add this line
//guest
builder.Services.AddScoped<IGuestStockService, GuestStockService>();
builder.Services.AddScoped<IGuestStockRepository, GuestStockRepository>();
builder.Services.AddScoped<IGuestStockSymbolRepository, GuestStockSymbolRepository>();
builder.Services.AddScoped<IGuestStockSymbolService, GuestStockSymbolService>();

builder.Services.AddScoped<IGuestStockGraphRepository, GuestStockGraphRepository>();
builder.Services.AddScoped<IGuestStockGraphService, GuestStockGraphService>();
builder.Services.AddHttpClient<GuestStockGraphRepository>();
builder.Services.AddHttpContextAccessor(); // Add this line

builder.Services.AddHttpClient<IFavStockService, FavStockService>();
builder.Services.AddScoped<IFavStockService, FavStockService>();
builder.Services.AddScoped<IFavStockRepository, FavStockRepository>();

// Load API key from configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Enable CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Set to true in production
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a-string-secret-at-least-256-bits-long")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
        // ValidIssuer = jwtIssuer,
        // ValidAudience = jwtAudience
    };
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StockAPI",
        Version = "v1",
        Description = "API for fetching stock data"
    });
});

var app = builder.Build();

// Enable session middleware
app.UseSession(); // Ensure this is added before UseRouting()

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockAPI v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication(); // Add this line if you have authentication middleware
app.UseAuthorization();
app.MapControllers();

app.Run();
