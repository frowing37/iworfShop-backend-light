using System.Text.Json;
using iworfShop_backend_light.Common;
using iworfShop_backend_light.Data;
using iworfShop_backend_light.Models.Dtos;
using iworfShop_backend_light.Models.Entities;
using iworfShop_backend_light.Services;
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
