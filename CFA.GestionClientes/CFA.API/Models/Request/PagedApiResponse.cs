namespace CFA.API.Models.Response;

public class PagedApiResponse<T> : ApiResponse<IEnumerable<T>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}