namespace iworfShop_backend_light.Common;

public struct IworfResult<T>
{
    public bool IsSuccess { get; set; }
    public IworfResultCode Code { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }
    
    public static IworfResult<T> Success(T data)
    {
        return new IworfResult<T>
        {
            IsSuccess = true,
            Code = IworfResultCode.Success,
            Data = data,
            ErrorMessage = null
        };
    }
    
    public static IworfResult<T> Fail(string error)
    {
        return new IworfResult<T>
        {
            IsSuccess = false,
            Code = IworfResultCode.Error,
            Data = default,
            ErrorMessage = error
        };
    }
}
