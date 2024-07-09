using FileSharingService.Response;

namespace FileSharingService.Middleware;

public class ErrorMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
		try
		{
			await next.Invoke(context);
		}
		catch (NullReferenceException ex)
		{
			var response = new BaseResponse<object>
			{
				Data = null,
				Error = ex.Message
			};

			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			await context.Response.WriteAsJsonAsync(response);		
		}
		catch(OperationCanceledException)
		{
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsync("Operation was canceled");
		}
	}
}