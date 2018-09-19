
namespace IdentityServer4Core.Controllers
{
    using IdentityServer4.Models;
    using IdentityServer4.Stores;
    using IdentityServer4Core.Data;
    using IdentityServer4Core.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    public class ResourceController : Controller
    {
        private readonly IResourceStore resourceStore;

        public ResourceController(IResourceStore resourceStore)
        {
            this.resourceStore = resourceStore;
        }

        public async Task<IActionResult> Get()
        {
            Resources resources = await this.resourceStore.GetAllResourcesAsync();

            return Ok(resources);
        }
    }
}