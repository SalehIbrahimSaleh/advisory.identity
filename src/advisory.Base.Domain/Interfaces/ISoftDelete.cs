namespace advisory.Base.Domain.Interfaces;
public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    public Guid? DeletedBy { get; set; }

    void MarkAsDeleted(Guid userId);
    void MarkAsNotDeleted();
}
