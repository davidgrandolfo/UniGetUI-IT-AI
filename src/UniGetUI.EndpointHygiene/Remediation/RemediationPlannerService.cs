using UniGetUI.EndpointHygiene.AI;
using UniGetUI.EndpointHygiene.Models;
using UniGetUI.EndpointHygiene.Policy;
using UniGetUI.EndpointHygiene.Risk;

namespace UniGetUI.EndpointHygiene.Remediation;

public sealed class RemediationPlannerService
{
    private readonly IRiskScoreService _riskScoreService;
    private readonly AIRecommendationOrchestrator _recommendationOrchestrator;
    private readonly IPolicyEngine _policyEngine;
    private readonly RollbackCapabilityResolver _rollbackCapabilityResolver;

    public RemediationPlannerService(
        IRiskScoreService riskScoreService,
        AIRecommendationOrchestrator recommendationOrchestrator,
        IPolicyEngine policyEngine,
        RollbackCapabilityResolver rollbackCapabilityResolver
    )
    {
        _riskScoreService = riskScoreService;
        _recommendationOrchestrator = recommendationOrchestrator;
        _policyEngine = policyEngine;
        _rollbackCapabilityResolver = rollbackCapabilityResolver;
    }

    public async Task<RemediationPlan> BuildPlanAsync(
        InventorySnapshot snapshot,
        RiskContext riskContext,
        PolicyDefinition policy,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        List<RemediationAction> actions = [];
        foreach (SoftwareInventoryItem item in snapshot.Items)
        {
            cancellationToken.ThrowIfCancellationRequested();

            RiskScoreResult risk = _riskScoreService.Score(item, riskContext);
            AIRecommendationResponse ai = await _recommendationOrchestrator.GetRecommendationAsync(
                new AIRecommendationRequest
                {
                    Item = item,
                    RiskScore = risk,
                },
                cancellationToken
            );

            RemediationActionType actionType = item.UpdateAvailable
                ? RemediationActionType.Update
                : RemediationActionType.Review;

            if (risk.Level == RiskLevel.Critical && !item.UpdateAvailable)
                actionType = RemediationActionType.Uninstall;

            RemediationAction action = new()
            {
                PackageId = item.PackageId,
                PackageName = item.Name,
                ActionType = actionType,
                RiskLevel = risk.Level,
                RequiresAdminApproval = risk.Level is RiskLevel.Medium or RiskLevel.High or RiskLevel.Critical,
                RollbackSupported = _rollbackCapabilityResolver.IsRollbackSupported(item),
                Reason = string.Join(" ", risk.Reasons),
                AiRecommendation = ai.Recommendation,
            };

            PolicyDecision decision = _policyEngine.Evaluate(item, action, policy);
            if (!decision.IsAllowed)
            {
                action.ActionType = RemediationActionType.Review;
                action.Reason = string.Join(" ", decision.Reasons);
            }

            if (decision.RequiresApproval)
                action.RequiresAdminApproval = true;

            actions.Add(action);
        }

        return new RemediationPlan
        {
            GeneratedAtUtc = DateTime.UtcNow,
            Actions = actions,
        };
    }
}
