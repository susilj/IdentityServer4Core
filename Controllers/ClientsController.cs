
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
    public class ClientsController : Controller
    {
        private readonly IClientStore clientStore;

        public ClientsController(IClientStore clientStore)
        {
            this.clientStore = clientStore;
        }

        [HttpGet("{clientId}", Name = "GetClientById")]
        public async Task<IActionResult> Get(string clientId)
        {
            Client client = await this.clientStore.FindClientByIdAsync(clientId);

            return Ok(client);
        }
    }
}