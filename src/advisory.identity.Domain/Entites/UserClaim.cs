using Microsoft.AspNetCore.Identity;

namespace advisory.identity.Domain.Entites;
public class UserClaim : IdentityUserClaim<Guid>
{
    public virtual User? User { get; set; }
}
