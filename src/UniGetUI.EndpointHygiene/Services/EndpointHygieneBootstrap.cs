using UniGetUI.EndpointHygiene.AI;
using UniGetUI.EndpointHygiene.Audit;
using UniGetUI.EndpointHygiene.Automation;
using UniGetUI.EndpointHygiene.Dashboard;
using UniGetUI.EndpointHygiene.Interfaces;
using UniGetUI.EndpointHygiene.Policy;
using UniGetUI.EndpointHygiene.Remediation;
using UniGetUI.EndpointHygiene.Risk;

namespace UniGetUI.EndpointHygiene.Services;

public static class EndpointHygieneBootstrap
{
    public static ISoftwareInventoryService Inventory { get; private set; } = new SoftwareInventoryService();
    public static IRiskScoreService RiskScoring { get; private set; } = new RiskScoreService();
    public static PolicyStore PolicyStore { get; private set; } = new();
    public static IPolicyEngine PolicyEngine { get; private set; } = new PolicyEngine();
    public static AIRecommendationOrchestrator Recommendations { get; private set; } =
        new(new RuleBasedRecommendationProvider());
    public static RollbackCapabilityResolver RollbackResolver { get; private set; } = new();
    public static RemediationPlannerService Planner { get; private set; } =
        new(
            RiskScoring,
            Recommendations,
            PolicyEngine,
            RollbackResolver
        );
    public static AuditLogService Audit { get; private set; } = new();
    public static ApprovalGateService ApprovalGate { get; private set; } = new();
    public static RemediationExecutor Executor { get; private set; } =
        new(ApprovalGate, Audit);
    public static EndpointHygieneMetricsService Metrics { get; private set; } = new(RiskScoring);

    public static void Initialize() { }
}
