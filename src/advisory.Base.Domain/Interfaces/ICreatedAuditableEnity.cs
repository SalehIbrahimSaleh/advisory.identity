namespace advisory.Base.Domain.Interfaces;
public interface ICreatedAuditableEnity
{
    public DateTimeOffset CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    void MarkAsCreated(Guid userId);
}
