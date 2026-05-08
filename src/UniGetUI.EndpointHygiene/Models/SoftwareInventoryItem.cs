namespace UniGetUI.EndpointHygiene.Models;

public sealed class SoftwareInventoryItem
{
    public string Name { get; set; } = "";
    public string PackageId { get; set; } = "";
    public string InstalledVersion { get; set; } = "";
    public string LatestVersion { get; set; } = "";
    public string ManagerName { get; set; } = "";
    public string SourceName { get; set; } = "";
    public string Publisher { get; set; } = "";
    public string? InstallDate { get; set; }
    public bool UpdateAvailable { get; set; }
    public string? InstallerHash { get; set; }
    public bool HasRecentFailure { get; set; }
}
