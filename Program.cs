using iworfShop_backend_light.Data;
using iworfShop_backend_light.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Lütfen 'Bearer [token]' formatında girin",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var jwtInit = scope.ServiceProvider.GetRequiredService<JwtOptionsService>();
    await jwtInit.GetTokenValidationParametersAsync();
}

app.Run();
