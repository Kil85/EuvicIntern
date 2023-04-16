using EuvicIntern.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace EuvicIntern.Middleware
{
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
            catch (LoginFailedException userNotFound)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(userNotFound.Message);
            }
            catch (NotFoundException notFound)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFound.Message);
            }
            catch (AuthorizationFailedException authorizationFailed)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(authorizationFailed.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Sth went wrong. Check logs");
            }
        }
    }
}
