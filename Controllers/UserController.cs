
namespace IdentityServer4Core.Controllers
{
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using IdentityServer4Core.Data;
    using IdentityServer4Core.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            ApplicationUser user = userManager.Users.FirstOrDefault(u => u.Id == userId);

            var userClaims = await this.userManager.GetClaimsAsync(user);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ApplicationUserClaims userClaims)
        {
            ApplicationUser user = userManager.Users.FirstOrDefault(u => u.Id == userClaims.UserId);
            
            // this.userManager.AddClaimsAsync(user, userClaims.UserClaims.Cast<Claim>());
            await this.userManager.AddClaimAsync(user, new Claim(userClaims.UserClaim.Type, userClaims.UserClaim.Value));

            return Created("/api/user/", userClaims.UserId);
        }

        [HttpPut("{userId}/{roleName}")]
        public async Task<IActionResult> Put(string userId, string roleName)
        {
            ApplicationUser user = userManager.Users.FirstOrDefault(u => u.Id == userId);
            
            await userManager.AddToRoleAsync(user, roleName);

            return Ok();
        }
    }
}