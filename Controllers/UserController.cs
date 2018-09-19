
namespace IdentityServer4Core.Controllers
{
    using IdentityServer4.Models;
    using IdentityServer4.Stores;
    using IdentityServer4Core.Data;
    using IdentityServer4Core.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Get()
        {
            // Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager = new Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>()
            // userManager.
            // List<ApplicationUser> users = this.context.Users.ToList();

            List<ApplicationUser> users = userManager.Users.ToList();

            return Ok(users);
        }
    }
}