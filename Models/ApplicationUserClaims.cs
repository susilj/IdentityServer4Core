
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4Core.Models
{
    public sealed class ApplicationUserClaims
    {
        public string UserId { get; set; }

        //public IEnumerable<ApplicationClaim> UserClaims { get; set; }
        public ApplicationClaim UserClaim { get; set; }
    } 
}