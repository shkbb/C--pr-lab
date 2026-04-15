using Microsoft.AspNetCore.Mvc.Filters;

namespace Library.Api.Filters;

public class ActionLoggingFilter : IActionFilter
{
    private readonly ILogger<ActionLoggingFilter> _logger;

    public ActionLoggingFilter(ILogger<ActionLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("Action '{ActionName}' is starting.", context.ActionDescriptor.DisplayName);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            _logger.LogWarning("Action '{ActionName}' threw an exception.", context.ActionDescriptor.DisplayName);
        }
        else
        {
            _logger.LogInformation("Action '{ActionName}' finished successfully.", context.ActionDescriptor.DisplayName);
        }
    }
}
