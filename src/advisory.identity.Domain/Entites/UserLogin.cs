using Microsoft.AspNetCore.Identity;

namespace advisory.identity.Domain.Entites;
public class UserLogin : IdentityUserLogin<Guid>
{
    public virtual User? User { get; set; }
}
