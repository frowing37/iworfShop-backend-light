using System.Security.Claims;
using iworfShop_backend_light.Common;
using iworfShop_backend_light.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iworfShop_backend_light.Controller;

[Route("api/[controller]")]
[ApiController]
public class TestController : IworfController
{
    private readonly IRedisClient  _redisClient;
    public TestController(IRedisClient redisClient)
    {
        _redisClient = redisClient;
    }

    [Authorize]
    [HttpGet("AuthTest")]
    public async Task<IActionResult> AuthTest()
    {
        var email = User.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

        var message = $"✅ Yetkili erişim başarılı. Kullanıcı ID: {userId}, Email: {email}";
        return Success(message);
    }

    [HttpGet("redisGetValue")]
    public async Task<IActionResult> RedisGetValue(string key)
    {
        var result = await _redisClient.GetValueAsync(key);
        return Success(result);
    }

    [HttpPost("redisSetValue")]
    public async Task<IActionResult> RedisSetValue(string key, string value)
    {
        var result = await _redisClient.SetValueAsync(key, value);
        return string.IsNullOrEmpty(result) ? Success("IYYEAHH") : Fail("tüh olmadı be");
    }
}
