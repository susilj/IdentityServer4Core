namespace IdentityServer4Core.Controllers
{
    using System.Threading.Tasks;
    using IdentityServer4Core.Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public sealed class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> manager;

        public RoleController(RoleManager<IdentityRole> manager)
        {
            this.manager = manager;
        }

        [HttpPost("{roleName}")]
        public async Task<IActionResult> Post(string roleName)
        {
            await this.manager.CreateAsync(new IdentityRole(roleName));

            return Ok();
        }
    }
}