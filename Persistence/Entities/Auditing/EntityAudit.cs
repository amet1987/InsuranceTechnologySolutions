namespace Persistence.Entities.Auditing;

public class EntityAudit
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string? HttpRequestType { get; set; }
}
