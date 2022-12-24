using advisory.Base.Domain.Models.Constants;
using advisory.Base.Domain.Models.ValueObjects;
using advisory.identity.Domain.Entites;
using advisory.identity.Domain.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace advisory.identity.Domain.Factory;
public class CreateUserFactory
{
    private string _email;
    private string? _phone;
    private string? _address;
    private GenderEnum? _gender;

    private readonly HashSet<UserRole> _roles = new();
    private readonly HashSet<UserClaim> _claims = new();

    public CreateUserFactory(string email)
    {
        _email = email;
    }
    public CreateUserFactory(string email, string? phone, string? address = null, GenderEnum? gender = null) : this(email)
    {
        _phone = phone;
        _address = address;
        _gender = gender;
    }

    public CreateUserFactory WithRole(List<Role> dbRoles, params RoleEnum[] roles)
    {
        foreach (var roleName in roles)
        {
            var role = dbRoles.FirstOrDefault(a => a.Name == roleName.ToString());
            if (role == null) continue;
            var userRole = new UserRole()
            {
                Role = role,
                RoleId = role.Id
            };
            _roles.Add(userRole);
        }

        return this;
    }
    public CreateUserFactory WithName(string firstName, string lastName)
    {
        AddClaim(ClaimKeys.FirstName, firstName);
        AddClaim(ClaimKeys.LastName, lastName);
        return this;
    }
    public CreateUserFactory WithImage(FileProperty? image)
    {
        var imageClaim = image is not null ? JsonConvert.SerializeObject(image) : string.Empty;
        AddClaim(ClaimKeys.Image, imageClaim);
        return this;
    }

    public User Build()
    {
        var user = new User()
        {
            Email = _email,
            PhoneNumber = _phone,
            Address = _address,
            Gender = _gender
        };

        foreach (var role in _roles)
        {
            user.AddUserRole(role);
        }
        foreach (var userClaim in _claims)
        {
            user.AddUserClaim(userClaim);
        }

        return user;
    }

    #region private operation
    private void AddClaim(string key, string value)
    {
        var claim = _claims.FirstOrDefault(a => a.ClaimType == key);
        if (claim is null)
        {
            claim = new UserClaim()
            {
                ClaimType = key,
                ClaimValue = value
            };
        }
        else
        {
            claim.ClaimValue = value;
        }
        _claims.Add(claim);
    }
    #endregion
}
