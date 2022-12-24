namespace advisory.Base.Domain.Interfaces;
public interface IModifiedAuditableEnity
{
    public DateTimeOffset LastModifiedDate { get; set; }
    public Guid LastModifiedBy { get; set; }
    void MarkAsModified(Guid userId);
}
