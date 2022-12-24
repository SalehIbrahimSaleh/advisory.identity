namespace advisory.Base.Domain.Models.ValueObjects;
public class FileProperty
{
    public string? Id { get; set; }
    public string? FileName { get; set; }
    public string? Url { get; set; }
    public bool IsExternal { get; set; }
    public FileProperty() { }
    public FileProperty(string id, string filename, string url, bool isExternal)
    {
        Id = id ?? Guid.NewGuid().ToString();
        FileName = filename ?? string.Empty;
        Url = url ?? string.Empty;
        IsExternal = isExternal;
    }
}
