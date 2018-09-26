
namespace IdentityServer4Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole
    {
        [StringLength(450)]
        public string TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
