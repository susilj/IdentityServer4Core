
namespace IdentityServer4Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [StringLength(450)]
        public string TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
