using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EuvicIntern.Authorization
{
    public class GetUserRequirment : IAuthorizationRequirement
    {
        public string Role { get; }

        public GetUserRequirment(string role)
        {
            Role = role;
        }
    }
}
