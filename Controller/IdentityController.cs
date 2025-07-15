using System.Text.Json;
using iworfShop_backend_light.Common;
using iworfShop_backend_light.Models.Dtos;
using iworfShop_backend_light.Models.Entities;
using iworfShop_backend_light.Services;
using Microsoft.AspNetCore.Mvc;

namespace iworfShop_backend_light.Controller;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : IworfController
{
    private readonly IIdentityService _identityService;
    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> SignIn(LoginModel model)
    {
        var control = await _identityService.CheckUserWithPassword(model);

        if (control.IsSuccess)
        {
            var user = JsonSerializer.Deserialize<User>(control.Data);
            var token = await _identityService.GenerateJwtToken(user);
            
            return Success(new UserResultModel
            {
                Token = token,
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
            });
        }
        else
        {
            if(control.Code == IworfResultCode.NotFound)
            {
                return NotFound(control.Message);
            }
            else
            {
                return Unauthorized(control.Message);
            }
        }
    }

    [HttpPost("signUp")]
    public async Task<IActionResult> SignUp(SignUpModel model)
    {
        var result = await _identityService.SignUp(model);

        if (result.IsSuccess)
        {
            var user = JsonSerializer.Deserialize<User>(result.Data);
            return Success(new UserResultModel
            {
                Token = null,
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
            });
        }
        else
        {
            return Fail(new UserResultModel(), result.Message);
        }
    }
}
