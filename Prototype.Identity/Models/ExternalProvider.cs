using System.ComponentModel.DataAnnotations.Schema;

namespace Prototype.Identity.Models
{
    [NotMapped]
    public class ExternalProvider
    {
        public string? DisplayName { get; set; }
        public string? AuthenticationScheme { get; set; }
    }
}
