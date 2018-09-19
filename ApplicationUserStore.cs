
namespace IdentityServer4Core
{
    using IdentityServer4Core.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationUserStore //: UserStore<TUser> 
        //where TUser : ApplicationUser
    {
        public ApplicationUserStore(DbContext context) //: base(context) 
        {
        }

        public int TenantId { get; set; }
    }
}