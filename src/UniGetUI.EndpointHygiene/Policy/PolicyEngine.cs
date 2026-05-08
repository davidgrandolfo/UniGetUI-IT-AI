using UniGetUI.EndpointHygiene.Models;
using UniGetUI.EndpointHygiene.Remediation;

namespace UniGetUI.EndpointHygiene.Policy;

public sealed class PolicyEngine : IPolicyEngine
{
    public PolicyDecision Evaluate(SoftwareInventoryItem item, RemediationAction action, PolicyDefinition policy)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(policy);

        List<string> reasons = [];
        bool allowed = true;
        bool requiresApproval = action.RequiresAdminApproval;

        if (policy.ApprovedSources.Count > 0 && !policy.ApprovedSources.Contains(item.SourceName))
        {
            allowed = false;
            reasons.Add("Source is not approved.");
        }

        if (policy.BlockedPublishers.Contains(item.Publisher))
        {
            allowed = false;
            reasons.Add("Publisher is blocked.");
        }

        if (policy.CriticalAppsRequiringApproval.Contains(item.PackageId)
            || policy.CriticalAppsRequiringApproval.Contains(item.Name))
        {
            requiresApproval = true;
            reasons.Add("Application requires approval by policy.");
        }

        if (policy.RequireRollbackWhenSupported && !action.RollbackSupported)
        {
            requiresApproval = true;
            reasons.Add("Rollback is required by policy but not supported.");
        }

        if (!IsWithinMaintenanceWindow(policy))
        {
            requiresApproval = true;
            reasons.Add("Outside configured maintenance window.");
        }

        return new PolicyDecision
        {
            IsAllowed = allowed,
            RequiresApproval = requiresApproval,
            Reasons = reasons,
        };
    }

    private static bool IsWithinMaintenanceWindow(PolicyDefinition policy)
    {
        if (!policy.MaintenanceWindowStartUtc.HasValue || !policy.MaintenanceWindowEndUtc.HasValue)
            return true;

        TimeOnly now = TimeOnly.FromDateTime(DateTime.UtcNow);
        TimeOnly start = policy.MaintenanceWindowStartUtc.Value;
        TimeOnly end = policy.MaintenanceWindowEndUtc.Value;

        if (start <= end)
            return now >= start && now <= end;

        return now >= start || now <= end;
    }
}
