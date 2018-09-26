
namespace IdentityServer4Core.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using IdentityServer4Core.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CustomUserManager : UserManager<ApplicationUser>
    {
        private readonly TenantManager tenantManager;

        public CustomUserManager(
            IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger,
            TenantManager tenantManager)
            : base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {
            this.tenantManager = tenantManager;
        }

        public virtual Task<ApplicationUser> FindByNameAndTenantAsync(string userName, string tenantId)
        {
            return Task.FromResult(base.Users.Where(u => u.NormalizedUserName == userName.ToUpper().Trim() && 
                                                    u.TenantId == tenantId)
                                                .SingleOrDefault());
        }

        public virtual Task<IdentityResult> CreateTenantUser(ApplicationUser user, Tenant tenant)
        {
            this.tenantManager.CreateTenant(tenant);

            string tenantId = this.tenantManager.GetTenantByName(tenant.Name).Id;

            user.TenantId = tenantId;

            return Task.FromResult(base.CreateAsync(user).Result);
        }
    }
}