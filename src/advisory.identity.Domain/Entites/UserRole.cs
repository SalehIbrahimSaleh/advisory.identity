using Microsoft.AspNetCore.Identity;

namespace advisory.identity.Domain.Entites;
public class UserRole : IdentityUserRole<Guid>
{
    public virtual User? User { get; set; }
    public virtual Role? Role { get; set; }
}
