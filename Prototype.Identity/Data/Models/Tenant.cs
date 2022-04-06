using System.ComponentModel.DataAnnotations;

namespace Prototype.Identity.Data.Models
{
    public class Tenant
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// This property is the UNIQUE name of a tenant. It can be
        /// used as a subdomain name in a web application (URL safe).
        /// </summary>
        public string TenancyName { get; set; }
        /// <summary>
        /// Display name of a tenant.
        /// </summary>
        public string? Name { get; set; }

        //public Tenant(string uniqueName, string displayName)
        //{
        //    TenancyName = uniqueName;
        //    Name = displayName;
        //}
    }
}
