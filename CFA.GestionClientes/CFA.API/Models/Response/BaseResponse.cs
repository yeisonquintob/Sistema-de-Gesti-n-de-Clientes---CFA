namespace CFA.API.Models.Response;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}