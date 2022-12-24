using advisory.identity.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace advisory.identity.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "IdentitySchema");

        builder.HasMany(x => x.Roles)
            .WithOne(x => x.User)
            .HasForeignKey(f => f.UserId)
            .IsRequired();

        builder.HasMany(x => x.Claims)
            .WithOne(x => x.User)
            .IsRequired();

        builder.Ignore(x => x.FirstName);
        builder.Ignore(x => x.LastName);
        builder.Ignore(x => x.Address);
        builder.Ignore(x => x.Image);
        builder.Ignore(x => x.Gender);
    }
}
