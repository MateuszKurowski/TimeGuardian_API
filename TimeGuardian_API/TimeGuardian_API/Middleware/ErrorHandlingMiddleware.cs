using TimeGuardian_API.Exceptions;

namespace TimeGuardian_API.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFoundException.Message);
        }
        catch (AlreadyExistsException alreadyExistException)
        {
            context.Response.StatusCode = 409;
            await context.Response.WriteAsync(alreadyExistException.Message);
        }
        catch (LoginException loginException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(loginException.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Oops! Looks like our server is trying to procrastinate instead of helping you. Please contact with administrator. In the meantime, perhaps it's a good time to get back to your tasks?");
        }
    }
}