namespace iworfShop_backend_light.Common;


public struct IworfResult
{
    public IworfResultCode Code { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsSuccess => Code == IworfResultCode.Success;
    
    public IworfResult(IworfResultCode code, string errorMessage = null)
    {
        Code = code;
        ErrorMessage = errorMessage;
    }
    
    public static IworfResult Success()
    {
        return new IworfResult(IworfResultCode.Success);
    }

    public static IworfResult Error(IworfResultCode code = IworfResultCode.NotFound, string errorMessage = null)
    {
        return new IworfResult(code, errorMessage);
    }
}
public partial struct IworfResult<T> where T : class
{
    public bool IsSuccess { get; set; } = true;
    public IworfResultCode Code { get; set; }
    public T? Data { get; set; }
    public string ErrorMessage { get; set; }

    private IworfResult(T data = default)
    {
        Data = data;
        Code = IworfResultCode.Success;
        ErrorMessage = null;
    }

    private IworfResult(T data, string errorMessage)
    {
        IsSuccess = false;
        Data = default;
        Code = IworfResultCode.NotFound;
        ErrorMessage = errorMessage;
    }

    private IworfResult(string authMessage)
    {
        IsSuccess = false;
        Data = default;
        Code = IworfResultCode.Unauthorized;
        ErrorMessage = authMessage;
    }
    
    public static IworfResult<T> Success(T data = default)
    {
        return new IworfResult<T>(data);
    }
    
    public static IworfResult<T> Fail(T data, string? errorMessage)
    {
        return new IworfResult<T>(data, errorMessage);
    }

    public static IworfResult<T> Unauthorized()
    {
        return new IworfResult<T>("Kimlik doğrulanamadı");
    }
}
