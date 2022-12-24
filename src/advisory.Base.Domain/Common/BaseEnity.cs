using advisory.Base.Domain.Interfaces;

namespace advisory.Base.Domain.Common;
public class BaseEnity : ISoftDelete
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedDate { get; set; }
    public Guid? DeletedBy { get; set; }

    public void MarkAsDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedBy = userId;
        DeletedDate = DateTimeOffset.UtcNow;
    }
    public void MarkAsNotDeleted()
    {
        IsDeleted = false;
    }
}
