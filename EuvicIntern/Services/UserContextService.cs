using System.Security.Claims;

namespace EuvicIntern.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal GetUser { get; }
        int? GetUserId { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public ClaimsPrincipal GetUser => _contextAccessor.HttpContext?.User;
        public int? GetUserId =>
            GetUser == null
                ? null
                : int.Parse(GetUser.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
