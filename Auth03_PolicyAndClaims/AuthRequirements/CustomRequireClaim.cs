using Microsoft.AspNetCore.Authorization;

namespace Auth03_PolicyAndClaims.AuthRequirements
{
    public partial class CustomRequireClaim : IAuthorizationRequirement
    {
        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }
}
