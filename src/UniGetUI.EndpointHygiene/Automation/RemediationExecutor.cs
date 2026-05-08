using UniGetUI.EndpointHygiene.Audit;
using UniGetUI.EndpointHygiene.Remediation;

namespace UniGetUI.EndpointHygiene.Automation;

public sealed class RemediationExecutor
{
    private readonly ApprovalGateService _approvalGate;
    private readonly AuditLogService _auditLogService;

    public RemediationExecutor(ApprovalGateService approvalGate, AuditLogService auditLogService)
    {
        _approvalGate = approvalGate;
        _auditLogService = auditLogService;
    }

    public async Task ExecutePlanAsync(RemediationPlan plan, string actor, CancellationToken cancellationToken = default)
    {
        foreach (RemediationAction action in plan.Actions)
        {
            bool requiresApproval = _approvalGate.RequiresValidation(action);
            string result = requiresApproval ? "PendingApproval" : "AutoRemediationPrepared";

            await _auditLogService.AppendAsync(
                new AuditLogEntry
                {
                    Actor = actor,
                    Action = action.ActionType.ToString(),
                    Target = action.PackageId,
                    Result = result,
                    Reason = action.Reason,
                    AiRecommendation = action.AiRecommendation,
                },
                cancellationToken
            );
        }
    }
}
