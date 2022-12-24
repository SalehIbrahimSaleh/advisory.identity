using Microsoft.AspNetCore.Identity;

namespace advisory.identity.Domain.Entites;
public class UserToken : IdentityUserToken<Guid>
{
    public virtual User? User { get; set; }
}
