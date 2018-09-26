
using System.Threading.Tasks;
using System.Linq;
using IdentityServer4Core.Models;
using IdentityServer4Core.Data;

namespace IdentityServer4Core.Managers
{
    //https://www.scottbrady91.com/ASPNET-Identity/Quick-and-Easy-ASPNET-Identity-Multitenancy
    //http://ericsmasal.com/2018/05/30/add-multi-tenancy-to-asp-net-core-identity/
    //http://ericsmasal.com/2018/06/05/capture-tenant-in-asp-net-core-2-0-web-api/
    public sealed class TenantManager
    {
        private readonly ApplicationDbContext context;

        public TenantManager(ApplicationDbContext context)
        {
            this.context = context;
        }

        public TenantResult CreateTenant(Tenant tenant)
        {
            this.context.Tenants.Add(tenant);

            int recordsAffected = this.context.SaveChanges();

            return new TenantResult { Succeeded = recordsAffected > 0 };
        }

        public Tenant GetTenantByName(string tenantName)
        {
            return this.context.Tenants.FirstOrDefault(t => t.Name.ToUpperInvariant() == tenantName.ToUpperInvariant());
        }
    }
}