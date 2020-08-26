using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Auth03_PolicyAndClaims.AuthRequirements
{
    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        public CustomRequireClaimHandler()
        {
            // If you want to communicate with database service and get claims info use this constructor
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireClaim requirement)
        {
            var hasClaim = context.User.Claims.Any(a => a.Type == requirement.ClaimType);

            if (hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
