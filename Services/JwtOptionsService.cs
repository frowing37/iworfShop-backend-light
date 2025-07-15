using System.Text;
using iworfShop_backend_light.Data;
using Microsoft.IdentityModel.Tokens;

namespace iworfShop_backend_light.Services;

public class JwtOptionsService
{
    private readonly IRedisClient _redis;

    public JwtOptionsService(IRedisClient redis)
    {
        _redis = redis;
    }

    public async Task<TokenValidationParameters> GetTokenValidationParametersAsync()
    {
        var key = await _redis.GetValueAsync("JwtConfig-Key");
        var issuer = await _redis.GetValueAsync("JwtConfig-Issuer");
        var audience = await _redis.GetValueAsync("JwtConfig-Audience");

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
        {
            key = await _redis.SetValueAsync("JwtConfig-Key", Guid.NewGuid().ToString());
            issuer = await _redis.SetValueAsync("JwtConfig-Issuer", "iworfShopAPI");
            audience = await _redis.SetValueAsync("JwtConfig-Audience", "iworfShopMobile");

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new Exception("redis jwt kısmı cortladı");
            }
        }

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    }
}
