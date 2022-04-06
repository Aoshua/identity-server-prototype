using System.ComponentModel.DataAnnotations.Schema;

namespace Prototype.Identity.Data.Models
{
    [NotMapped]
    public class ExternalProvider
    {
        //[Key]
        //public int Id { get; set; }
        public string? DisplayName { get; set; }
        public string? AuthenticationScheme { get; set; }
    }
}
