namespace UniGetUI.EndpointHygiene.Audit;

public sealed class AuditLogEntry
{
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public string Actor { get; set; } = "";
    public string Action { get; set; } = "";
    public string Target { get; set; } = "";
    public string Result { get; set; } = "";
    public string Reason { get; set; } = "";
    public string AiRecommendation { get; set; } = "";
}
