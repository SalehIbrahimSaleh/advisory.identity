using advisory.Base.Domain.Interfaces;

namespace advisory.Base.Domain.Common;
public class AuditableEntity : BaseEnity, ICreatedAuditableEnity, IModifiedAuditableEnity
{
    public DateTimeOffset CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }

    public DateTimeOffset LastModifiedDate { get; set; }
    public Guid LastModifiedBy { get; set; }

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
}
