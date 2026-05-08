using UniGetUI.EndpointHygiene.Remediation;
using UniGetUI.EndpointHygiene.Risk;

namespace UniGetUI.EndpointHygiene.Automation;

public sealed class ApprovalGateService
{
    public bool RequiresValidation(RemediationAction action)
    {
        if (action.RequiresAdminApproval)
            return true;

        return action.RiskLevel is RiskLevel.Medium or RiskLevel.High or RiskLevel.Critical;
    }
}
