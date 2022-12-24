using advisory.Base.Domain.Interfaces;
using advisory.Base.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace advisory.Base.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private const string _userId = "sub";
    protected ApplicationDbContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor? httpContextAccessor = null) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);

        modelBuilder.GetOnlyNotDeletedEntities();
    }

    public Task<int> SaveChangesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        CheckAndUpdateEntities(userId);
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = new Guid(GetUserId());
        CheckAndUpdateEntities(userId);
        return base.SaveChangesAsync(cancellationToken);
    }

    public void CheckAndUpdateEntities(Guid userId)
    {
        ChangeTracker
            .Entries<ICreatedAuditableEnity>()
            .Where(a => a.State == EntityState.Added).ToList()
            .ForEach(entity => entity.Entity.MarkAsCreated(userId));

        ChangeTracker.Entries<IModifiedAuditableEnity>()
            .Where(a => a.State == EntityState.Modified).ToList()
            .ForEach(en => en.Entity.MarkAsModified(userId));

        ChangeTracker.Entries<ISoftDelete>()
            .Where(a => a.State == EntityState.Deleted || a.State == EntityState.Added).ToList()
            .ForEach(en =>
            {
                if (en.State == EntityState.Deleted)
                    en.Entity.MarkAsDeleted(userId);
                else
                    en.Entity.MarkAsNotDeleted();
            });
    }
    public string GetUserId()
    {
        var user = _httpContextAccessor?.HttpContext?.User;
        if (user?.Identity is null || user.Identity.IsAuthenticated == false)
            throw new UnauthorizedAccessException("Unauthorized.");

        var userId = user.Claims.FirstOrDefault(a => a.Type == _userId)?.Value;
        return userId ??
            throw new UnauthorizedAccessException("Unauthorized.");
    }
}
