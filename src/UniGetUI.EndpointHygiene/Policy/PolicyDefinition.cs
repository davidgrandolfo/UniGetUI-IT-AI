namespace UniGetUI.EndpointHygiene.Policy;

public sealed class PolicyDefinition
{
    public ISet<string> ApprovedSources { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public ISet<string> BlockedPublishers { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public ISet<string> CriticalAppsRequiringApproval { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public int MinimumPackageAgeDaysBeforeUpdate { get; set; }
    public bool RequireRollbackWhenSupported { get; set; }
    public TimeOnly? MaintenanceWindowStartUtc { get; set; }
    public TimeOnly? MaintenanceWindowEndUtc { get; set; }
}
