using System.Text.Json;
using iworfShop_backend_light.Common;
using iworfShop_backend_light.Models.Dtos;
using iworfShop_backend_light.Models.Entities;
using iworfShop_backend_light.Services;
using Microsoft.AspNetCore.Mvc;

namespace iworfShop_backend_light.Controller;

[Route("api/[controller]")]
[ApiController]
public class IdentityController
{
    private readonly IdentityService _identityService;

    public IdentityController(IdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("login")]
    public async Task<IworfResult<UserResultModel>> SignIn(LoginModel model)
    {
        var control = await _identityService.CheckUserWithPassword(model);

        if (control.IsSuccess)
        {
            var user = JsonSerializer.Deserialize<User>(control.Data);
            var token = await _identityService.GenerateJwtToken(user);
            
            return IworfResult<UserResultModel>.Success(new UserResultModel
            {
                Token = token,
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
            });
        }
        else
        {
            return IworfResult<UserResultModel>.Fail(control.Message);
        }
    }

    [HttpPost("signUp")]
    public async Task<IworfResult<UserResultModel>> SignUp(SignUpModel model)
    {
        var result = await _identityService.SignUp(model);

        if (result.IsSuccess)
        {
            return IworfResult<UserResultModel>.Success(new UserResultModel());
        }
        else
        {
            return IworfResult<UserResultModel>.Fail(result.Message);
        }
    }
}
