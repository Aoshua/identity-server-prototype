using Microsoft.AspNetCore.Identity;

namespace Prototype.Identity.Data.Models
{
    /// <summary>
    /// For now, this is a direct copy of AbpZero User.
    /// Eventually many of these properties will be stripped away,
    /// but I'm just trying to keep mapping simple for now. 
    /// </summary>
    public class User : IdentityUser<long>
    {
        public int? ProfilePictureId { get; set; }
        public bool ShouldChangePasswordOnNextLogin { get; set; }
        public DateTime? SignInTokenExpireTimeUtc { get; set; }
        public string? SignInToken { get; set; }
        public string? GoogleAuthenticatorKey { get; set; }
        public int? TopMenuId { get; set; }
        public int LeftMenuId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifiedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string? AuthenticationSource { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string? EmailConfirmationCode { get; set; }
        public string? PasswordResetCode { get; set; }
        public bool IsActive { get; set; }
    }
}
