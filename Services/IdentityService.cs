using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using iworfShop_backend_light.Common;
using iworfShop_backend_light.Common.Helpers;
using iworfShop_backend_light.Data;
using iworfShop_backend_light.Models;
using iworfShop_backend_light.Models.Dtos;
using iworfShop_backend_light.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace iworfShop_backend_light.Services;

public interface IIdentityService
{
    Task<string> GenerateJwtToken(User user);
    Task<QueryResult> SignUp(SignUpModel model);
    Task<QueryResult> CheckUserWithPassword(LoginModel model);
}

public class IdentityService : IIdentityService
{
    private readonly SqlLiteClient  _sqlLiteClient;
    private readonly IRedisClient _redisClient;

    public IdentityService(SqlLiteClient sqlLiteClient, IRedisClient redisClient)
    {
        _sqlLiteClient = sqlLiteClient;
        _redisClient = redisClient;
    }
    public async Task<string> GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        try
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(await _redisClient.GetValueAsync("JwtConfig-Key")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenAttributes = new JwtSecurityToken(
                issuer: await _redisClient.GetValueAsync("JwtConfig-Issuer"),
                audience: await _redisClient.GetValueAsync("JwtConfig-Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenAttributes);
            return token;
        }
        catch (Exception e)
        {
            throw new Exception($"Token generation failed = {e.Message}");
        }
    }
    public async Task<QueryResult> SignUp(SignUpModel model)
    {
        var control = await _sqlLiteClient.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (control is not null)
        {
            return new QueryResult() { IsSuccess = false, Message = "Bu email ile bir kullanıcı kaydı zaten mevcut.", };
        }
        else
        {
            try
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Surname = model.Surname,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    HashPassword = TextHasher.HashIt(model.Password),
                };
                _sqlLiteClient.Users.Add(newUser);
                await _sqlLiteClient.SaveChangesAsync();

                return new QueryResult()
                    { IsSuccess = true, Message = "IYYEEAHHHH", Data = JsonSerializer.Serialize(newUser) };
            }
            catch (Exception e)
            {
                return new QueryResult()
                    { IsSuccess = false, Message = e.Message };
            }
        }
        
    }
    public async Task<QueryResult> CheckUserWithPassword(LoginModel model)
    {
        var user = _sqlLiteClient.Users.Where(x => x.Email == model.Email).FirstOrDefault();

        if (user is null)
        {
            return new QueryResult { IsSuccess = false, Message = "Bu mail ile bir kayıtlı bir kullanıcı bulunmamaktadır.", Code = IworfResultCode.NotFound};
        }

        var control = TextHasher.Verify(model.Password, user.HashPassword);

        if (control)
        {
            return new QueryResult() { IsSuccess = true, Message = "IYYEEEAAAAHHHHHH", Data = JsonSerializer.Serialize(user), Code = IworfResultCode.Success};
        }
        else
        {
            return new QueryResult() { IsSuccess = false, Message = "Kullanıcı için girilen şifre yanlış." , Code = IworfResultCode.Error };
        }
    }
}