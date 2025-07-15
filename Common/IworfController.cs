using Microsoft.AspNetCore.Mvc;

namespace iworfShop_backend_light.Common;

public class IworfController : ControllerBase
{
    protected IworfResult Success()
    {
        return IworfResult.Success();
    }

    protected IActionResult Success<T>(T data) where T : class
    {
        var result = IworfResult<T>.Success(data);
        return Ok(result);
    }
    
    protected IActionResult Fail(IworfResultCode code = IworfResultCode.NotFound,
        string? customErrorMessage = null)
    {
        var result = IworfResult.Error(code, customErrorMessage);
        return BadRequest(result);
    }

    protected IActionResult Fail<TData>(TData? data, string? customErrorMessage = null) where TData : class
    {
        var result = IworfResult<TData>.Fail(data, customErrorMessage);
        return BadRequest(result);
    }

    protected IActionResult Unauthorized(string? message = "Kimlik doğrulama başarısız.")
    {
        var result = IworfResult.Error(IworfResultCode.Unauthorized, message);
        return Unauthorized(result);
    }
    
    protected IActionResult NotFound(string? message = "İstenen kaynak bulunamadı.")
    {
        var result = IworfResult.Error(IworfResultCode.NotFound, message);
        return NotFound(result);
    }
}