using advisory.Base.Domain.Interfaces;

namespace advisory.Base.Domain.Common;
public class ActivityEntity : BaseEnity, ICreatedAuditableEnity
{
    public DateTimeOffset CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }

    public void MarkAsCreated(Guid userId)
    {
        CreatedBy = userId;
        CreatedDate = DateTimeOffset.UtcNow;
    }
}
