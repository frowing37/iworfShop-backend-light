using Microsoft.AspNetCore.Mvc;

namespace iworfShop_backend_light.Common;

public class IworfController : ControllerBase
{
    protected IworfResult Success()
    {
        return IworfResult.Success();
    }

    protected IworfResult<T> Success<T>(T data) where T : class
    {
        return IworfResult<T>.Success(data);
    }
    
    protected IworfResult Fail(IworfResultCode code = IworfResultCode.SystemError,
        string? customErrorMessage = null)
    {
        return IworfResult.Error(code, customErrorMessage);
    }

    protected IworfResult<TData> Fail<TData>(TData? data, string? customErrorMessage = null) where TData : class
    {
        return IworfResult<TData>.Fail(data, customErrorMessage);
    }
}