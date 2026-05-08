using UniGetUI.EndpointHygiene.Models;

namespace UniGetUI.EndpointHygiene.Remediation;

public sealed class RollbackCapabilityResolver
{
    public bool IsRollbackSupported(SoftwareInventoryItem item)
    {
        return !string.IsNullOrWhiteSpace(item.ManagerName)
            && !item.ManagerName.Equals("Scoop", StringComparison.OrdinalIgnoreCase);
    }
}
