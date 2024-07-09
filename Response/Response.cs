namespace FileSharingService.Response;

public class Response<T>
{
    public T? Data { get; set; }
    public string? Error { get; set; }
}