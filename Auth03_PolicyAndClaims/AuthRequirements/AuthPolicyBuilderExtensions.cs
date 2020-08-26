using Microsoft.AspNetCore.Authorization;

namespace Auth03_PolicyAndClaims.AuthRequirements
{
    public static class AuthPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType)
        {
            return builder.AddRequirements(new CustomRequireClaim(claimType));
        }
    }
}
