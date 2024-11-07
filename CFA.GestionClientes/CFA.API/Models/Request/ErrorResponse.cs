namespace CFA.API.Models.Response;

public class ErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string[] Errors { get; set; }
}