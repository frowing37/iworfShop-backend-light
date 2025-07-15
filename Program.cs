using iworfShop_backend_light.Data;
using iworfShop_backend_light.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

SQLitePCL.Batteries.Init();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<SqlLiteClient>(options =>
    options.UseSqlite("Data Source=app.db"));
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379"));

builder.Services.AddScoped<JwtOptionsService>();
builder.Services.AddScoped<IRedisClient, RedisClient>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JwtScheme", options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = async context =>
            {
                var jwtOptionsService = context.HttpContext.RequestServices.GetRequiredService<JwtOptionsService>();
                var tokenValidationParams = await jwtOptionsService.GetTokenValidationParametersAsync();
                
                context.Options.TokenValidationParameters = tokenValidationParams;
                
                await Task.CompletedTask;
            }
        };
    });

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Controller'ları route etmek için gerekli

app.Run();
