using UniGetUI.EndpointHygiene.Dashboard;
using UniGetUI.EndpointHygiene.Models;
using UniGetUI.EndpointHygiene.Policy;
using UniGetUI.EndpointHygiene.Remediation;
using UniGetUI.EndpointHygiene.Risk;
using UniGetUI.EndpointHygiene.Services;

namespace UniGetUI.Interface;

public sealed class IpcHygieneResponse<T>
{
    public DateTime GeneratedAtUtc { get; set; } = DateTime.UtcNow;
    public T Data { get; set; } = default!;
}

public static class IpcEndpointHygieneApi
{
    public static async Task<IpcHygieneResponse<IReadOnlyList<SoftwareInventoryItem>>> GetInventoryAsync()
    {
        InventorySnapshot snapshot = await EndpointHygieneBootstrap.Inventory.BuildSnapshotAsync();
        return new IpcHygieneResponse<IReadOnlyList<SoftwareInventoryItem>>
        {
            GeneratedAtUtc = snapshot.GeneratedAtUtc,
            Data = snapshot.Items,
        };
    }

    public static async Task<IpcHygieneResponse<RemediationPlan>> GetRemediationPlanAsync()
    {
        InventorySnapshot snapshot = await EndpointHygieneBootstrap.Inventory.BuildSnapshotAsync();
        PolicyDefinition policy = await EndpointHygieneBootstrap.PolicyStore.LoadAsync();
        RiskContext context = BuildRiskContext(policy, snapshot.Items);
        RemediationPlan plan = await EndpointHygieneBootstrap.Planner.BuildPlanAsync(snapshot, context, policy);

        return new IpcHygieneResponse<RemediationPlan>
        {
            GeneratedAtUtc = plan.GeneratedAtUtc,
            Data = plan,
        };
    }

    public static async Task<IpcHygieneResponse<EndpointHygieneSnapshot>> GetDashboardAsync()
    {
        InventorySnapshot snapshot = await EndpointHygieneBootstrap.Inventory.BuildSnapshotAsync();
        PolicyDefinition policy = await EndpointHygieneBootstrap.PolicyStore.LoadAsync();
        RiskContext context = BuildRiskContext(policy, snapshot.Items);
        EndpointHygieneSnapshot dashboard = EndpointHygieneBootstrap.Metrics.Build(snapshot, context);

        return new IpcHygieneResponse<EndpointHygieneSnapshot>
        {
            GeneratedAtUtc = dashboard.GeneratedAtUtc,
            Data = dashboard,
        };
    }

    public static string GetAuditLogPath()
    {
        return EndpointHygieneBootstrap.Audit.GetAuditPath();
    }

    private static RiskContext BuildRiskContext(
        PolicyDefinition policy,
        IReadOnlyList<SoftwareInventoryItem> items
    )
    {
        HashSet<string> failedPackages = new(
            items.Where(i => i.HasRecentFailure).Select(i => i.PackageId),
            StringComparer.OrdinalIgnoreCase
        );

        return new RiskContext
        {
            ApprovedSources = policy.ApprovedSources,
            CriticalApps = policy.CriticalAppsRequiringApproval,
            BlockedPublishers = policy.BlockedPublishers,
            FailedPackages = failedPackages,
            HighRiskManagers = new HashSet<string>(HygieneDefaults.HighRiskManagers, StringComparer.OrdinalIgnoreCase),
        };
    }
}
