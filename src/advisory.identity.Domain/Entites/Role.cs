using Microsoft.AspNetCore.Identity;

namespace advisory.identity.Domain.Entites;
public class Role : IdentityRole<Guid>
{
    private string _displayName;

    public Role(string name, string displayName)
    {
        Name = name;
        NormalizedName = Name.ToUpper();
        _displayName = displayName;
    }

    private readonly HashSet<RoleClaim> _claims = new();
    private readonly HashSet<UserRole> _roles = new();

    public virtual User? User { get; set; }
    public virtual IReadOnlyCollection<RoleClaim> Claims => _claims;
    public virtual IReadOnlyCollection<UserRole> Roles => _roles;

    public string DisplayName { get => _displayName; set => _displayName = value; }
}
