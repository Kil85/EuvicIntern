using Microsoft.AspNetCore.Mvc.Filters;
using NLog.Web.LayoutRenderers;
using System.Security.Claims;

namespace EuvicIntern.IntegrationTests.Fakers
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            var claimsPrincipe = new ClaimsPrincipal();
            claimsPrincipe.AddIdentity(
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(ClaimTypes.Role, "Admin")
                    }
                )
            );
            context.HttpContext.User = claimsPrincipe;
            await next();
        }
    }
}
