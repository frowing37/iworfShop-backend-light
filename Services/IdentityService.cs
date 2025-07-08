using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using iworfShop_backend_light.Common;
using iworfShop_backend_light.Common.Helpers;
using iworfShop_backend_light.Data;
using iworfShop_backend_light.Models.Dtos;
using iworfShop_backend_light.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace iworfShop_backend_light.Services;

public interface IIdentityService
{
    Task<string> GenerateJwtToken(User user);
    Task<bool> SignUp(SignUpModel model);
    Task<QueryResult> CheckUserWithPassword(LoginModel model);
}

public class IdentityService : IIdentityService
{
    private readonly IConfiguration _config;
    private readonly SqlLiteClient  _sqlLiteClient;

    public IdentityService(IConfiguration configuration, SqlLiteClient sqlLiteClient)
    {
        _config = configuration;
        _sqlLiteClient = sqlLiteClient;
    }

    public async Task<string> GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Task<bool> SignUp(SignUpModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<QueryResult> CheckUserWithPassword(LoginModel model)
    {
        var user = _sqlLiteClient.Users.Where(x => x.Email == model.Email).FirstOrDefault();

        if (user is null)
        {
            return new QueryResult { IsSuccess = false, Message = "Bu mail ile bir kayıtlı bir kullanıcı bulunmamaktadır." };
        }

        var control = TextHasher.Verify(model.Password, user.HashPassword);

        if (control)
        {
            return new QueryResult() { IsSuccess = true, Message = "IYYEEEAAAAHHHHHH" };
        }
        else
        {
            return new QueryResult() { IsSuccess = false, Message = "Kullanıcı için girilen şifre yanlış." };
        }
    }
}