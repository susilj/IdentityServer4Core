
namespace IdentityServer4Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using IdentityServer4Core.Models;

    [Table("AspNetTenants")]
    public class Tenant
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        public string DomainName { get; set; }

        [StringLength(50)]
        [Required]
        public string Code { get; set; }

        public bool IsDefault { get; set; }

        public virtual IEnumerable<ApplicationUser> Users { get; set; }
    }
}