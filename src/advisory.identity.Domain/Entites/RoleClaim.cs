using Microsoft.AspNetCore.Identity;

namespace advisory.identity.Domain.Entites;
public class RoleClaim : IdentityRoleClaim<Guid>
{
    public virtual Role? Role { get; set; }
}
