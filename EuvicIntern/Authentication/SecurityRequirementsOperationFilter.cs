using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EuvicIntern.Authentication
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo
                .GetCustomAttributes(inherit: true)
                .Union(context.MethodInfo.DeclaringType.GetCustomAttributes(inherit: true))
                .OfType<AuthorizeAttribute>();

            if (attributes.Any())
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                operation.Security.Add(
                    new OpenApiSecurityRequirement { { scheme, Array.Empty<string>() } }
                );
            }
        }
    }
}
