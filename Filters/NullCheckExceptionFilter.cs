using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FileSharingService.Filters;

public class NullCheckExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        string actionName = context.ActionDescriptor.DisplayName!;
        string exceptionMessage = context.Exception.Message;
        string exceptionStack = context.Exception.StackTrace!;

        context.Result = new ContentResult
        {
            Content = $"In the method {actionName} an exception has occurred: \n {exceptionMessage} \n {exceptionStack}"
        };

        context.ExceptionHandled = true;
    }
}
