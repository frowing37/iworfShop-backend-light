namespace iworfShop_backend_light.Common;

public class QueryResult
{
    public IworfResultCode? Code { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string Data { get; set; }
}