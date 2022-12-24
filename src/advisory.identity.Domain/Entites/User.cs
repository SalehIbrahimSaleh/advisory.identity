using advisory.Base.Domain.Interfaces;
using advisory.Base.Domain.Models.Constants;
using advisory.Base.Domain.Models.ValueObjects;
using advisory.identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace advisory.identity.Domain.Entites;
public class User : IdentityUser<Guid>, ICreatedAuditableEnity, IModifiedAuditableEnity, ISoftDelete
{\
    private readonly HashSet<UserClaim> _claims = new();
    private readonly HashSet<UserRole> _roles = new();
    private readonly HashSet<UserLogin> _logins = new();
    private readonly HashSet<UserToken> _tokens = new();

    public string? FirstName
    {
        get
        {
            var firtNameClaim = Claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.FirstName);
            return firtNameClaim is null ? AddUserClaim(ClaimKeys.FirstName, string.Empty).ClaimValue
                                        : firtNameClaim.ClaimValue;
        }
        set
        {
            var firtNameClaim = Claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.FirstName);
            firtNameClaim ??= AddUserClaim(ClaimKeys.FirstName, value ?? string.Empty);
            firtNameClaim.ClaimValue = value;
        }
    }
    public string? LastName
    {
        get
        {
            var lastNameClaim = Claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.LastName);
            return lastNameClaim is null ? AddUserClaim(ClaimKeys.LastName, string.Empty).ClaimValue
                                        : lastNameClaim.ClaimValue;
        }
        set
        {
            var lastNameClaim = Claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.LastName);
            lastNameClaim ??= AddUserClaim(ClaimKeys.LastName, value ?? string.Empty);
            lastNameClaim.ClaimValue = value;
        }
    }
    public FileProperty? Image
    {
        get
        {
            var imageClaim = Claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.Image);
            imageClaim ??= AddUserClaim(ClaimKeys.Image, string.Empty);
            return imageClaim.ClaimValue is null or "" ? null :
                JsonConvert.DeserializeObject<FileProperty>(imageClaim.ClaimValue);
        }
        set
        {
            var claimValue = JsonConvert.SerializeObject(value);
            var imageClaim = Claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.Image);
            imageClaim ??= AddUserClaim(ClaimKeys.Image, claimValue);
            imageClaim.ClaimValue = claimValue;
        }
    }
    public GenderEnum? Gender
    {
        get
        {
            var genderClaim = _claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.Gender);
            if (genderClaim is null)
            {
                genderClaim = AddUserClaim(ClaimKeys.Gender, string.Empty);
                return null;
            }
            return (GenderEnum)Enum.Parse(typeof(GenderEnum), genderClaim.ClaimValue);
        }
        set
        {
            var genderClaim = _claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.Gender);
            genderClaim ??= AddUserClaim(ClaimKeys.Gender, value.ToString() ?? string.Empty);
            genderClaim.ClaimValue = value.ToString();
        }
    }

    public string? Address
    {
        get
        {
            var addressClaim = Claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.Address);
            return addressClaim is null ? AddUserClaim(ClaimKeys.Address, string.Empty).ClaimValue
                                        : addressClaim.ClaimValue;
        }
        set
        {
            var addressClaim = Claims.FirstOrDefault(a => a.ClaimType == ClaimKeys.Address);
            addressClaim ??= AddUserClaim(ClaimKeys.Address, value ?? string.Empty);
            addressClaim.ClaimValue = value;
        }
    }

    #region Auditable member
    public DateTimeOffset CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }

    public DateTimeOffset LastModifiedDate { get; set; }
    public Guid LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedDate { get; set; }
    public Guid? DeletedBy { get; set; }
    #endregion

    public virtual IReadOnlyCollection<UserClaim> Claims => _claims;
    public virtual IReadOnlyCollection<UserRole> Roles => _roles;
    public virtual IReadOnlyCollection<UserLogin> Logins => _logins;
    public virtual IReadOnlyCollection<UserToken> Tokens => _tokens;


    #region public operation
    public void AddUserRole(UserRole role)
    {
        if (_roles.Any(a => a.RoleId == role.RoleId)) return;
        _roles.Add(role);
    }
    public void AddUserClaim(UserClaim userClaim)
    {
        if (_claims.Any(a => a.ClaimType == userClaim.ClaimType)) return;
        userClaim.User = this;
        userClaim.UserId = this.Id;
        _claims.Add(userClaim);
    }
    public UserClaim AddUserClaim(string claimKey, string claimValue)
    {
        var newClaim = _claims.FirstOrDefault(a => a.ClaimType == claimKey);
        if (newClaim is not null) return newClaim;

        newClaim = new UserClaim()
        {
            UserId = Id,
            ClaimType = claimKey,
            ClaimValue = claimValue
        };
        _claims.Add(newClaim);
        return newClaim;
    }
    #endregion

    #region auditable operation
    public void MarkAsCreated(Guid userId)
    {
        CreatedBy = userId;
        CreatedDate = DateTimeOffset.UtcNow;
    }

    public void MarkAsModified(Guid userId)
    {
        LastModifiedBy = userId;
        LastModifiedDate = DateTimeOffset.UtcNow;
    }
    public void MarkAsDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedBy = userId;
        DeletedDate = DateTimeOffset.UtcNow;
    }
    #endregion
}
