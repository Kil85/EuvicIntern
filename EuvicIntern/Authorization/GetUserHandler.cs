using EuvicIntern.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace EuvicIntern.Authorization
{
    public class GetUserHandler : AuthorizationHandler<GetUserRequirment, User>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetUserRequirment requirement,
            User resource
        )
        {
            if (requirement.Role == "Admin")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userId = int.Parse(
                context.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value
            );

            if (userId == resource.Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
