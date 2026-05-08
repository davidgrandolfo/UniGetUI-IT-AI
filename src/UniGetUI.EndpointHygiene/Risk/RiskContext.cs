namespace UniGetUI.EndpointHygiene.Risk;

public sealed class RiskContext
{
    public ISet<string> ApprovedSources { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public ISet<string> CriticalApps { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public ISet<string> HighRiskManagers { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public ISet<string> BlockedPublishers { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public ISet<string> FailedPackages { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public ISet<string> CveExposedPackages { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
}
